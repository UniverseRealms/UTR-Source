using System.Collections.Concurrent;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds.logic;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private const int PingPeriod = 3000;
        public const int DcThreshold = 12000;

        private long _pingTime = -1;
        private long _pongTime = -1;

        private int _cnt;

        private long _sum;
        public long TimeMap { get; private set; }

        private long _latSum;
        public int Latency { get; private set; }
        public bool HatchlingNotification { get; set; }

        public int LastClientTime = -1;
        public long LastServerTime = -1;

        private readonly ConcurrentQueue<long> _shootAckTimeout =
            new ConcurrentQueue<long>();

        private readonly ConcurrentQueue<long> _updateAckTimeout =
            new ConcurrentQueue<long>();

        private readonly ConcurrentQueue<long> _gotoAckTimeout =
            new ConcurrentQueue<long>();

        private readonly ConcurrentQueue<int> _move =
            new ConcurrentQueue<int>();

        private readonly ConcurrentQueue<int> _clientTimeLog =
            new ConcurrentQueue<int>();

        private readonly ConcurrentQueue<int> _serverTimeLog =
            new ConcurrentQueue<int>();

        private bool KeepAlive()
        {
            if (_pingTime == -1)
            {
                _pingTime = Manager.Core.getTotalTickCount() - PingPeriod;
                _pongTime = Manager.Core.getTotalTickCount();
            }

            // check for disconnect timeout
            if (Manager.Core.getTotalTickCount() - _pongTime > DcThreshold)
            {
                _client.Disconnect("Connection timeout. (KeepAlive)");
                return false;
            }

            // check for shootack timeout
            if (_shootAckTimeout.TryPeek(out var timeout))
            {
                if (Manager.Core.getTotalTickCount() > timeout)
                {
                    _client.Disconnect("Connection timeout. (ShootAck)");
                    return false;
                }
            }

            // check for updateack timeout
            if (_updateAckTimeout.TryPeek(out timeout))
            {
                if (Manager.Core.getTotalTickCount() > timeout)
                {
                    _client.Disconnect("Connection timeout. (UpdateAck)");
                    return false;
                }
            }

            // check for gotoack timeout
            if (_gotoAckTimeout.TryPeek(out timeout))
            {
                if (Manager.Core.getTotalTickCount() > timeout)
                {
                    _client.Disconnect("Connection timeout. (GotoAck)");
                    return false;
                }
            }

            if (Manager.Core.getTotalTickCount() - _pingTime < PingPeriod)
                return true;

            // send ping
            _pingTime = Manager.Core.getTotalTickCount();
            _client.SendPacket(new Ping()
            {
                Serial = (int)Manager.Core.getTotalTickCount()
            });
            return UpdateOnPing();
        }

        public void Pong(Pong pongPkt)
        {
            _cnt++;

            _sum += Manager.Core.getTotalTickCount() - pongPkt.Time;
            TimeMap = _sum / _cnt;

            _latSum += (Manager.Core.getTotalTickCount() - pongPkt.Serial) / 2;
            Latency = (int)_latSum / _cnt;

            _pongTime = Manager.Core.getTotalTickCount();
        }

        private bool UpdateOnPing()
        {
            // renew account lock
            try
            {
                if (!Manager.Database.RenewLock(_client.Account))
                    _client.Disconnect("RenewLock failed. (Pong)");
            }
            catch
            {
                _client.Disconnect("RenewLock failed. (Timeout)");
                return false;
            }

            // save character
            if (Owner != null && !Owner.Deleted && !(Owner is Test))
            {
                SaveToCharacter();

                if (_client != null && _client.Character != null) _client.Character.FlushAsync();
            }
            return true;
        }

        public long C2STime(int clientTime)
        {
            return clientTime + TimeMap;
        }

        public long S2CTime(int serverTime)
        {
            return serverTime - TimeMap;
        }

        public void AwaitShootAck(long serverTime)
        {
            _shootAckTimeout.Enqueue(serverTime + DcThreshold);
        }

        public void ShootAckReceived()
        {
            if (!_shootAckTimeout.TryDequeue(out _))
            {
                _client.Disconnect("One too many ShootAcks");
            }
        }

        public void AwaitUpdateAck(long serverTime)
        {
            _updateAckTimeout.Enqueue(serverTime + DcThreshold);
        }

        public void UpdateAckReceived()
        {
            if (!_updateAckTimeout.TryDequeue(out _))
            {
                _client.Disconnect("One too many UpdateAcks");
            }
        }

        public void AwaitGotoAck(long serverTime)
        {
            _gotoAckTimeout.Enqueue(serverTime + DcThreshold);
        }

        public void GotoAckReceived()
        {
            if (!_gotoAckTimeout.TryDequeue(out _))
            {
                _client.Disconnect("One too many GotoAcks");
            }
        }

        public void AwaitMove(int tickId)
        {
            _move.Enqueue(tickId);
        }

        public void MoveReceived(Move pkt)
        {
            if (!_move.TryDequeue(out var tickId))
            {
                _client.Disconnect("One too many MovePackets");
                return;
            }

            if (tickId != pkt.TickId)
            {
                _client.Disconnect("[NewTick -> Move] TickIds don't match");
                return;
            }

            if (pkt.TickId > TickId)
            {
                _client.Disconnect("[NewTick -> Move] Invalid tickId");
                return;
            }

            LastClientTime = pkt.Time;
            LastServerTime = Manager.Core.getTotalTickCount();
        }
    }
}
