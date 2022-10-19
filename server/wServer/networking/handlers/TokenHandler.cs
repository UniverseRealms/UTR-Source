using log4net;
using System;
using System.Collections.Generic;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.networking.packets.outgoing;

namespace wServer.networking.handlers
{
    internal class TokenHandler : PacketHandlerBase<RequestToken>
    {
        public override PacketId ID => PacketId.REQUESTTOKEN;

        protected override void HandlePacket(Client client, RequestToken packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
            Handle(client, packet);
        }

        private void Handle(Client client, RequestToken packet)
        {
            string[] tokens = client.Manager.Config.serverSettings.tokens;
            client.TokenId = new Random().Next(tokens.Length);
            client.IsAir = packet.IsAir;
            string token = tokens[client.TokenId];
            if (packet.Token != "idk") token = "fail_xd";
            client.SendPacket(new ReceiveToken() { Token = token });
        }
    }
}