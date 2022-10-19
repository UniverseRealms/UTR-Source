using common;
using log4net;

namespace server
{
    public class ChatManager
    {
        private readonly InterServerChannel _interServer;

        public ChatManager(InterServerChannel interServer)
        {
            _interServer = interServer;

            // listen to chat communications
            _interServer.AddHandler<ChatMsg>(Channel.Chat, HandleChat);
        }

        private void HandleChat(object sender, InterServerEventArgs<ChatMsg> e)
        {
            switch (e.Content.Type)
            {
                case ChatType.Party:
                    {
                        var from = _interServer.Database.ResolveIgn(e.Content.From);
                        Program.Debug(typeof(ChatManager), $"<{from} -> Party> {e.Content.Text}");
                        break;
                    }
            }
        }
    }
}
