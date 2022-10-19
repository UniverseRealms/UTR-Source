using common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.networking.server;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds.logic;

namespace wServer.networking
{
    public enum ProtocolState
    {
        Disconnected,
        Connected,
        Handshaked,
        Queued,
        Ready
    }

    public partial class Client
    {
        public RealmManager Manager { get; }

        private static readonly byte[] ServerKey = { 0x61, 0x2a, 0x80, 0x6c, 0xac, 0x78, 0x11, 0x4b, 0xa5, 0x01, 0x3c, 0xb5, 0x31 };
        private static byte[] _clientKey = { 0x81, 0x1f, 0x50, 0x39, 0x1f, 0xb4, 0x55, 0x89, 0x9c, 0xa9, 0xd7, 0x4b, 0x72 };

        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }

        private readonly Server _server;
        private readonly CommHandler _handler;

        private volatile ProtocolState _state;

        public ProtocolState State
        {
            get => _state;
            internal set => _state = value;
        }

        public int Id { get; internal set; }
        public string GUID { get; internal set; }
        public DbAccount Account { get; internal set; }
        public DbChar Character { get; internal set; }
        public Player Player { get; internal set; }
        public bool EventNotification { get; internal set; }
        public wRandom Random { get; internal set; }

        public int TokenId = -1;
        public bool IsAir = false;

        //Temporary connection state
        internal int TargetWorld = -1;

        public Socket Skt { get; private set; }
        public string IP { get; private set; }
        public bool IsLagging { get; private set; }
        public bool IgnoreAllyProjectiles { get; internal set; }
        public bool IgnoreAllyDamageText { get; internal set; }

        internal readonly object DcLock = new object();

        public Client(Server server, RealmManager manager,
            SocketAsyncEventArgs send, SocketAsyncEventArgs receive,
            byte[] clientKey)
        {
            _server = server;
            Manager = manager;
            _clientKey = clientKey;

            ReceiveKey = new RC4(_clientKey);
            SendKey = new RC4(ServerKey);

            _handler = new CommHandler(this, send, receive);
        }

        public void Reset()
        {
            ReceiveKey = new RC4(_clientKey);
            SendKey = new RC4(ServerKey);

            Id = 0; // needed so that inbound packets that are currently queued are discarded.
            Account = null;
            Character = null;
            Player = null;

            // reset client ping/pong values
            _pingTime = -1;
            _pongTime = -1;

            _handler.Reset();
        }

        public void BeginHandling(Socket skt)
        {
            Skt = skt;

            try { IP = ((IPEndPoint)skt.RemoteEndPoint).Address.ToString(); }
            catch { IP = ""; }

            _handler.BeginHandling(Skt);
        }

        public void SendPacket(Packet pkt)
        {
            if (Monitor.TryEnter(DcLock, new TimeSpan(0, 0, 1)))
                try { if (State != ProtocolState.Disconnected) _handler.SendPacket(pkt); }
                finally { Monitor.Exit(DcLock); }
        }

        public void SendPackets(IEnumerable<Packet> pkts)
        {
            if (Monitor.TryEnter(DcLock, new TimeSpan(0, 0, 1)))
                try { if (State != ProtocolState.Disconnected) _handler.SendPackets(pkts); }
                finally { Monitor.Exit(DcLock); }
        }

        public bool IsReady()
        {
            switch (State)
            {
                case ProtocolState.Disconnected:
                case ProtocolState.Ready when Player?.Owner == null:
                    return false;
            }

            return true;
        }

        public bool CheckForLag() => _handler.IsLagging();

        internal void ProcessPacket(Packet pkt)
        {
            lock (DcLock)
            {
                if (State == ProtocolState.Disconnected)
                    return;

                try
                {
                    if (!PacketHandlers.Handlers.TryGetValue(pkt.ID, out var handler))
                        return;

                    handler.Handle(this, (IncomingMessage)pkt);
                }
                catch (Exception e)
                {
                    Program.Debug(typeof(Client), $"Packet Id {pkt.ID} Error: {e.ToString()}", true);
                    Disconnect("Packet handling error.");
                }
            }
        }

        public void Reconnect(Reconnect pkt)
        {
            if (Account == null)
            {
                Disconnect("Tried to reconnect an client with a null account...");
                return;
            }

            Manager.ConMan.AddReconnect(this, pkt);
            SendPacket(pkt);
        }

        public async void SendFailure(string text, int errorId = 0)
        {
            SendPacket(new Failure()
            {
                ErrorId = errorId,
                ErrorDescription = text
            });

            await Task.Delay(1000);

            Disconnect();
        }

        public async void SendFailureDialog(string title, string description)
        {
            // Note: Client is programmed to check the build parameter
            // of the json object. If it doesn't match what the client
            // has, the error dialog will be an update client dialog
            // instead.

            var jsonMsg = new FailureJsonDialogMessage()
            {
                build = Manager.Config.serverSettings.version,
                title = title,
                description = description
            };
            SendPacket(new Failure()
            {
                ErrorId = Failure.JsonDialogDisconnect,
                ErrorDescription = JsonConvert.SerializeObject(jsonMsg)
            });

            await Task.Delay(1000);

            Disconnect();
        }

        public void Disconnect(string reason = "")
        {
            lock (DcLock)
            {
                if (State == ProtocolState.Disconnected)
                    return;

                State = ProtocolState.Disconnected;

                if (Account != null)
                    try
                    {
                        Save();
                    }
                    catch (Exception e)
                    {
                        Program.Debug(typeof(Client), e.ToString(), true);
                    }

                Manager.Disconnect(this);
                _server.Disconnect(this);
            }
        }

        private void Save() // only when disconnect
        {
            var acc = Account;

            if (Character == null || Player == null || Player.Owner is Test)
            {
                Manager.Database.ReleaseLock(acc);
                return;
            }

            Player.SaveToCharacter();

            if (!acc.Hidden && acc.AccountIdOverrider == 0)
                acc.RefreshLastSeen();

            acc.FlushAsync();

            Manager.Database.SaveCharacter(acc, Character, Player.FameCounter.ClassStats, true).GetAwaiter();
            Manager.Database.ReleaseLock(acc);
        }

        public void Dispose() { }
    }
}
