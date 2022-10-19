using DiscordWebhook;

namespace common
{
    public class DiscordLogging
    {
        private bool Enabled;
        private string[] HackerCache = new string[5];
        private int HC = 0;

        public DiscordLogging(bool enabled)
        {
            Enabled = enabled;
        }

        /*public void ModClientLog(string info)
        {
            HackerCache[HC] = info;
            HC++;
            if (HC >= 5)
            {
                LogToDiscord(WebHooks.ModdedClient, "Haxors Log", $"[{string.Join(",", HackerCache)}]");
                HackerCache = new string[5];
                HC = 0;
            }
        }*/

        public void LogToDiscord(Webhook hook, string name, string content)
        {
            if (!Enabled) return;

            hook.PostData(new WebhookObject()
            {
                username = name,
                content = content
            });
        }
    }

    public class WebHooks
    {
        //To add these simply go to a discord text chat
        //Add a new web hook
        //and replace the WEBHOOKURLHERE with it
        public static Webhook Test = new Webhook("WEBHOOKURLHERE");
        public static Webhook ModdedClient = new Webhook("WEBHOOKURLHERE");
        public static Webhook ModCommand = new Webhook("WEBHOOKURLHERE");
        public static Webhook GiveItem = new Webhook("WEBHOOKURLHERE");
        public static Webhook Special = new Webhook("WEBHOOKURLHERE");
        public static Webhook Trade = new Webhook("WEBHOOKURLHERE");
        public static Webhook Loot = new Webhook("WEBHOOKURLHERE");
    }
}