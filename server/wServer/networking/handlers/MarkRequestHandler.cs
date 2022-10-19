using System.Linq;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    internal class MarkRequestHandler : PacketHandlerBase<MarkRequest>
    {
        public override PacketId ID => PacketId.MARKREQUEST;

        protected override void HandlePacket(Client client, MarkRequest packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet));
        }

        private int[] Marks = { 0, 1, 2, 3, 4, 5, 6, 12 };
        private int[] Nodes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        private void Handle(Player player, MarkRequest p)
        {
            if (!player.MarksEnabled)
            {
                player.SendError("You do not have marks enabled on this character!");
                return;
            }

            switch (p.Type_)
            {
                case 0:
                    BuyNode(player, p);
                    break;

                case 1:
                    BuyMark(player, p);
                    break;
            }
        }

        private void BuyMark(Player player, MarkRequest p)
        {
            int cost = (player.Mark == 0) ? 40 : 20;

            if (player.Onrane < cost)
            {
                player.SendError("Insufficent Onrane to buy this Mark!");
                return;
            }

            if (!Marks.Contains(p.Id_))
            {
                player.SendError("Invalid Mark Id");
                return;
            }

            if (p.Slot_ != 0)
            {
                player.SendError("Invalid Mark Slot");
                return;
            }
            if (p.Id_ == player.Mark)
            {
                player.SendError("smh, stop trying to buy the same mark");
                return;
            }

            player.Client.Manager.Database.UpdateOnrane(player.Client.Account, -cost);
            player.Onrane -= cost;
            player.ForceUpdate(player.Onrane);

            player.Mark = p.Id_;
            player.SendHelp("You have activated this Mark!");
            player.UpdateMarks(player);
        }

        private void BuyNode(Player player, MarkRequest p)
        {
            int id = 0;
            switch (p.Slot_)
            {
                case 1:
                    id = player.Node1;
                    break;

                case 2:
                    id = player.Node2;
                    break;

                case 3:
                    id = player.Node3;
                    break;

                case 4:
                    id = player.Node4;
                    break;
            }

            int cost = (id == 0) ? 15 : 7;

            if (player.Onrane < cost)
            {
                player.SendError("Insufficent Onrane to buy this Mark!");
                return;
            }

            if (!Nodes.Contains(p.Id_))
            {
                player.SendError("Invalid Mark Id");
                return;
            }

            if (p.Slot_ < 1 || p.Slot_ > 4)
            {
                player.SendError("Invalid Slot");
                return;
            }

            if (p.Id_ == id)
            {
                player.SendError("smh, stop trying to buy the same node");
                return;
            }

            player.Client.Manager.Database.UpdateOnrane(player.Client.Account, -cost);
            player.Onrane -= cost;
            player.ForceUpdate(player.Onrane);

            switch (p.Slot_)
            {
                case 1:
                    player.Node1 = p.Id_;
                    break;

                case 2:
                    player.Node2 = p.Id_;
                    break;

                case 3:
                    player.Node3 = p.Id_;
                    break;

                case 4:
                    player.Node4 = p.Id_;
                    break;
            }
            player.SendHelp("You have activated this Node!");
            player.UpdateMarks(player);
        }
    }
}
