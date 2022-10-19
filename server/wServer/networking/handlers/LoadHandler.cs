using System;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.worlds.logic;

namespace wServer.networking.handlers
{
    internal class LoadHandler : PacketHandlerBase<Load>
    {
        public override PacketId ID => PacketId.LOAD;

        protected override void HandlePacket(Client client, Load packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
            Handle(client, packet);
        }

        private void Handle(Client client, Load packet)
        {
            if (client.State != ProtocolState.Handshaked)
                return;
            client.Character = client.Manager.Database
                                .LoadCharacter(client.Account, packet.CharId);
            if (client.Character != null)
            {
                if (client.Character.Dead)
                {
                    client.SendFailure("Character is dead.",
                        Failure.MessageWithDisconnect);
                }
                else
                {
                    //Console.WriteLine(packet.ClientSize);
                    /*if(Program.Config.serverSettings.checkClient)
                        if (Math.Round(packet.ClientSize, 13) != Math.Round(Program.Config.serverSettings.clientSize, 13))
                        {
                            client.SendFailureDialog("Invalid Client", "Please use https://dangergun.github.io/UTReborn.swf?build=release to play");
                            return;
                        }*/
                    var target = client.Manager.Worlds[client.TargetWorld];

                    client.Player = target is Test ?
                        new Player(client, false) :
                        new Player(client);

                    client.SendPacket(new CreateSuccess()
                    {
                        CharId = client.Character.CharId,
                        ObjectId = target.EnterWorld(client.Player)
                    });

                    client.State = ProtocolState.Ready;
                    client.Manager.ConMan.ClientConnected(client);
                }
            }
            else
            {
                client.SendFailure("Failed to load character.",
                    Failure.MessageWithDisconnect);
            }
        }
    }
}