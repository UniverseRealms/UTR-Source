using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    public class PlayerMessage
    {
        public Player Player { get; private set; }
        public string Message { get; private set; }
        public long Time { get; private set; }

        public PlayerMessage(Player player, string msg)
        {
            Player = player;
            Message = msg;
            Time = player.Manager.Core.getTotalTickCount();
        }
    }

    internal class PlayerTextHandler : PacketHandlerBase<PlayerText>
    {
        public override PacketId ID => PacketId.PLAYERTEXT;

        protected override void HandlePacket(Client client, PlayerText packet)
        {
            client.Manager.Core.addPendingAction(() => Handle(client.Player, packet.Text));
        }

        private void Handle(Player player, string text)
        {
            if (player?.Owner == null || text.Length > 512) return;

            var manager = player.Manager;

            // check for commands before other checks
            if (text[0] == '/') manager.Commands.Execute(player, text);
            else
            {
                if (!player.NameChosen)
                {
                    player.SendError("Please choose a name before chatting.");
                    return;
                }

                if (player.Muted)
                {
                    player.SendError("Muted. You can not talk at this time.");
                    return;
                }

                if (player.CompareAndCheckSpam(text, manager.Core.getTotalTickCount())) return;

                // save message for mob behaviors
                player.Owner.ChatReceived(player, text);
                manager.Chat.Say(player, text);
            }
        }
    }
}
