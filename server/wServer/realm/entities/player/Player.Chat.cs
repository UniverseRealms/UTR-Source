using System;
using System.Text.RegularExpressions;
using wServer.networking.packets.outgoing;

namespace wServer.realm.entities
{
    partial class Player
    {
        private static Regex nonAlphaNum = new Regex("[^a-zA-Z0-9 ]", RegexOptions.CultureInvariant);
        private static Regex repetition = new Regex("(.)(?<=\\1\\1)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private int LastMessageDeviation = Int32.MaxValue;
        private string LastMessage = "";
        private long LastMessageTime = 0;
        private bool Spam = false;

        public bool CompareAndCheckSpam(string message, long time)
        {
            if (time - LastMessageTime < 500)
            {
                LastMessageTime = time;
                if (Spam)
                {
                    return true;
                }
                else
                {
                    Spam = true;
                    return false;
                }
            }

            string strippedMessage = nonAlphaNum.Replace(message, "").ToLower();
            strippedMessage = repetition.Replace(strippedMessage, "");

            if (time - LastMessageTime > 10000)
            {
                LastMessageDeviation = LevenshteinDistance(LastMessage, strippedMessage);
                LastMessageTime = time;
                LastMessage = strippedMessage;
                Spam = false;
                return false;
            }
            else
            {
                int deviation = LevenshteinDistance(LastMessage, strippedMessage);
                LastMessageTime = time;
                LastMessage = strippedMessage;

                if (LastMessageDeviation <= LengthThreshold(LastMessage.Length) && deviation <= LengthThreshold(message.Length))
                {
                    LastMessageDeviation = deviation;
                    if (Spam)
                    {
                        return true;
                    }
                    else
                    {
                        Spam = true;
                        return false;
                    }
                }
                else
                {
                    LastMessageDeviation = deviation;
                    Spam = false;
                    return false;
                }
            }
        }

        public static int LengthThreshold(int length)
        {
            return length > 4 ? 3 : 0;
        }

        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; d[i, 0] = i++) ;

            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        public void SendInfo(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = text
            });
        }

        public void SendInfoFormat(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = string.Format(text, args)
            });
        }

        public void SendError(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Error*",
                Txt = text
            });
        }

        public void SendErrorFormat(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Error*",
                Txt = string.Format(text, args)
            });
        }

        public void SendClientText(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Client*",
                Txt = text
            });
        }

        public void SendClientTextFormat(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Client*",
                Txt = string.Format(text, args)
            });
        }

        public void SendHelp(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Help*",
                Txt = text
            });
        }

        public void SendHelpFormat(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "*Help*",
                Txt = string.Format(text, args)
            });
        }

        public void SendEnemy(string name, string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "#" + name,
                Txt = text
            });
        }

        public void SendGuildAnnounce(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = text,
                TextColor = 0x00b300
            });
        }

        public void SendGuildAnnounceFormat(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = string.Format(text, args),
                TextColor = 0x00b300
            });
        }

        public void SendSacredAnnounce(string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = string.Format(text, args),
                TextColor = 0xEE82EE
            });
        }

        public void SendEnemyFormat(string name, string text, params object[] args)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "#" + name,
                Txt = string.Format(text, args)
            });
        }

        public void SendText(string sender, string text, int nameColor = 0x123456, int textColor = 0x123456)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = sender,
                Txt = text,
                NameColor = nameColor,
                TextColor = textColor
            });
        }

        internal void TellReceived(int objId, int stars, int admin, string from, string to, string text)
        {
            Client.SendPacket(new Text()
            {
                ObjectId = objId,
                BubbleTime = 10,
                NumStars = stars,
                Admin = admin,
                Name = from,
                Recipient = to,
                Txt = text
            });
        }

        internal void Invited(int objId, string inviter, string dungeon)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = "You've been invited by " + inviter + " to join them in " + dungeon + "! Use /accept to be summoned into the dungeon!",
                TextColor = 0xDE01FC
            });
        }

        internal void AnnouncementReceived(string text)
        {
            _client.Player.SendInfo(string.Concat("<ANNOUNCEMENT> ", text));

            /*client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "@ANNOUNCEMENT",
                Txt = text
            });*/
        }

        internal void GuildReceived(int objId, int stars, int admin, string from, string text)
        {
            Client.SendPacket(new Text()
            {
                ObjectId = objId,
                BubbleTime = 10,
                NumStars = stars,
                Admin = admin,
                Name = from,
                Recipient = "*Guild*",
                Txt = text
            });
        }

        internal void PartyReceived(int objId, int stars, int admin, string from, string text)
        {
            Client.SendPacket(new Text()
            {
                ObjectId = objId,
                BubbleTime = 10,
                NumStars = stars,
                Admin = admin,
                Name = from,
                Recipient = "*Party*",
                Txt = text,
                TextColor = 0xDE01FC
            });
        }
        internal void StaffReceived(int objId, int stars, int admin, string from, string text)
        {
            Client.SendPacket(new Text()
            {
                ObjectId = objId,
                BubbleTime = 10,
                NumStars = stars,
                Admin = admin,
                Name = from,
                Recipient = "*Staff*",
                Txt = text,
                TextColor = 0xADD8E6
            });
        }
		public void SendWorldBossOne(string text)
		{
			_client.SendPacket(new Text()
			{
				BubbleTime = 0,
				NumStars = -1,
				Name = "Filisha",
				Txt = text,
				TextColor = 0x8B0000
			});
		}
		public void SendWorldBossTwo(string text)
		{
			_client.SendPacket(new Text()
			{
				BubbleTime = 0,
				NumStars = -1,
				Name = null,
				Txt = text,
				TextColor = 0xFF00FF
			});
		}
		public void ClassSetNotif(string text)
        {
            _client.SendPacket(new Text()
            {
                BubbleTime = 0,
                NumStars = -1,
                Name = "",
                Txt = text,
                NameColor = 0x00ECFF,
                TextColor = 0x00ECFF
            });
        }
    }
}