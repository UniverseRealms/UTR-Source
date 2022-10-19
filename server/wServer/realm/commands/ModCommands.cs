using common;
using common.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;

namespace wServer.realm.commands
{
    #region ModerationCommands

    internal class AdminChat : Command
    {
        public AdminChat() : base("achat", alias: "a")
        {
        }

        protected override bool Process(Player player, string args)
        {
            if (player.Client.Account.LegacyRank < 70)
            {
                player.SendError("OI");
                return false;
            }

            return player.Manager.Chat.Staff(player, args);
        }
    }
        internal class KickCommand : Command
    {
        public KickCommand() : base("kick", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string args)
        {
            foreach (var i in player.Manager.Clients.Keys)
            {
                if (i.Account.Name.EqualsIgnoreCase(args))
                {
                    // probably if someone is hidden doesn't want to be kicked, so we leave it as before
                    if (i.Account.Hidden)
                        break;

                    Program.DL.LogToDiscord(WebHooks.ModCommand, "Kick Log", $"Kick Log: [{player.Name}] has kicked [{i.Account.Name}]");
                    i.Disconnect();
                    player.SendInfo("Player disconnected!");

                    return true;
                }
            }
            player.SendError($"Player '{args}' could not be found!");
            return false;
        }
    }

    internal class MuteCommand : Command
    {
        private static readonly Regex CmdParams = new Regex(@"^(\w+)( \d+)?$", RegexOptions.IgnoreCase);

        private readonly RealmManager _manager;

        public MuteCommand(RealmManager manager) : base("mute", Perms.Mod)
        {
            _manager = manager;
            _manager.DbEvents.Expired += HandleUnMute;
        }

        protected override bool Process(Player player, string args)
        {
            var match = CmdParams.Match(args);
            if (!match.Success)
            {
                player?.SendError("Usage: /mute <player name> <time out in minutes>\\n" +
                                 "Time parameter is optional. If left out player will be muted until unmuted.");
                return false;
            }

            // gather arguments
            var name = match.Groups[1].Value;
            var id = _manager.Database.ResolveId(name);
            var acc = _manager.Database.GetAccount(id);
            int timeout;
            if (string.IsNullOrEmpty(match.Groups[2].Value))
            {
                timeout = -1;
            }
            else
            {
                int.TryParse(match.Groups[2].Value, out timeout);
            }

            // run through checks
            if (id == 0 || acc == null)
            {
                player?.SendError("Account not found!");
                return false;
            }
            if (acc.IP == null)
            {
                player?.SendError("Account has no associated IP address. Player must login at least once before being muted.");
                return false;
            }
            if (acc.IP.Equals(player?.Client.Account.IP))
            {
                player?.SendError("Mute failed. That action would cause yourself to be muted (IPs are the same).");
                return false;
            }
            if (acc.Admin)
            {
                player?.SendError("Cannot mute other admins.");
                return false;
            }

            Program.DL.LogToDiscord(WebHooks.ModCommand, "Mute Log", $"Mute Log: [{player.Name}] has muted [{name}] for {timeout} minutes");

            // mute player if currently connected
            foreach (var client in _manager.Clients.Keys
                        .Where(c => c.Player != null && c.IP.Equals(acc.IP) && !c.Player.Client.Account.Admin))
            {
                client.Player.Muted = true;
                client.Player.ApplyConditionEffect(ConditionEffectIndex.Muted);
            }

            if (player != null)
            {
                if (timeout > 0)
                    _manager.Chat.SendInfo(id, "You have been muted by " + player.Name + " for " + timeout + " minutes.");
                else
                    _manager.Chat.SendInfo(id, "You have been muted by " + player.Name + ".");
            }

            // mute ip address
            if (timeout < 0)
            {
                _manager.Database.Mute(acc.IP);
                player?.SendInfo(name + " successfully muted indefinitely.");
            }
            else
            {
                _manager.Database.Mute(acc.IP, TimeSpan.FromMinutes(timeout));
                player?.SendInfo(name + " successfully muted for " + timeout + " minutes.");
            }

            return true;
        }

        private void HandleUnMute(object entity, DbEventArgs expired)
        {
            var key = expired.Message;

            if (!key.StartsWith("mutes:"))
                return;

            foreach (var client in _manager.Clients.Keys.Where(c =>
                c.Player != null && c.IP.Equals(key.Substring(6)) && !c.Player.Client.Account.Admin))
            {
                client.Player.Muted = false;
                client.Player.ApplyConditionEffect(ConditionEffectIndex.Muted, 0);
                client.Player.SendInfo("You are no longer muted. Please do not spam. Thank you.");
            }
        }
    }

    internal class UnMuteCommand : Command
    {
        public UnMuteCommand() : base("unmute", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendError("Usage: /unmute <player name>");
                return false;
            }

            // gather needed info
            var id = player.Manager.Database.ResolveId(name);
            var acc = player.Manager.Database.GetAccount(id);

            // run checks
            if (id == 0 || acc == null)
            {
                player.SendError("Account not found!");
                return false;
            }
            if (acc.IP == null)
            {
                player.SendError("Account has no associated IP address. Player must login at least once before being unmuted.");
                return false;
            }

            Program.DL.LogToDiscord(WebHooks.ModCommand, "Unmute Log", $"Unmute Log: [{player.Name}] has unmuted [{name}]");

            // unmute ip address
            player.Manager.Database.IsMuted(acc.IP).ContinueWith(t =>
            {
                if (!t.IsCompleted)
                {
                    player.SendInfo("Db access error while trying to unmute.");
                    return;
                }

                if (t.Result)
                {
                    player.Manager.Database.Mute(acc.IP, TimeSpan.FromSeconds(1));
                    player.SendInfo(name + " successfully unmuted.");
                }
                else
                {
                    player.SendInfo(name + " wasn't muted...");
                }
            });

            // expire event will handle unmuting of connected players
            return true;
        }
    }
    internal class BanAccountCommand : Command
    {
        public BanAccountCommand() : base("ban", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string args)
        {
            BanInfo bInfo;
            if (args.StartsWith("{"))
            {
                bInfo = Utils.FromJson<BanInfo>(args);
            }
            else
            {
                bInfo = new BanInfo();

                // validate command
                var rgx = new Regex(@"^(\w+) (.+)$");
                var match = rgx.Match(args);
                if (!match.Success)
                {
                    player.SendError("Usage: /ban <account id or name> <reason>");
                    return false;
                }

                // get info from args
                bInfo.Name = match.Groups[1].Value;
                if (!int.TryParse(bInfo.Name, out bInfo.accountId))
                {
                    bInfo.accountId = player.Manager.Database.ResolveId(bInfo.Name);
                }
                bInfo.banReasons = match.Groups[2].Value;
                bInfo.banLiftTime = -1;
            }

            // run checks
            if (Database.GuestNames.Any(n => n.ToLower().Equals(bInfo.Name?.ToLower())))
            {
                player.SendError("If you specify a player name to ban, the name needs to be unique.");
                return false;
            }
            if (bInfo.accountId == 0)
            {
                player.SendError("Account not found...");
                return false;
            }
            if (string.IsNullOrWhiteSpace(bInfo.banReasons))
            {
                player.SendError("A reason must be provided.");
                return false;
            }
            var acc = player.Manager.Database.GetAccount(bInfo.accountId);
            if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
            {
                player.SendError("Cannot ban players of equal or higher rank than yourself.");
                return false;
            }

            Program.DL.LogToDiscord(WebHooks.ModCommand, "Ban Log", $"Ban Log: [{player.Name}] has banned [{acc.Name}] for {bInfo.banReasons}");

            // ban player + disconnect if currently connected
            player.Manager.Database.Ban(bInfo.accountId, bInfo.banReasons, bInfo.banLiftTime);
            var target = player.Manager.Clients.Keys
                .SingleOrDefault(c => c.Account != null && c.Account.AccountId == bInfo.accountId);
            target?.Disconnect();

            player.SendInfo(!string.IsNullOrEmpty(bInfo.Name) ?
                $"{bInfo.Name} successfully banned." :
                "Ban successful.");
            return true;
        }

        private class BanInfo
        {
            public int accountId;
            public string Name;
            public string banReasons;
            public int banLiftTime;
        }
    }

    internal class BanIPCommand : Command
    {
        public BanIPCommand() : base("banip", Perms.Mod, alias: "ipban")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var manager = player.Manager;
            var db = manager.Database;

            // validate command
            var rgx = new Regex(@"^(\w+) (.+)$");
            var match = rgx.Match(args);
            if (!match.Success)
            {
                player.SendError("Usage: /banip <account id or name> <reason>");
                return false;
            }

            // get info from args
            var idstr = match.Groups[1].Value;
            if (!int.TryParse(idstr, out int id))
            {
                id = db.ResolveId(idstr);
            }
            var reason = match.Groups[2].Value;

            // run checks
            if (Database.GuestNames.Any(n => n.ToLower().Equals(idstr.ToLower())))
            {
                player.SendError("If you specify a player name to ban, the name needs to be unique.");
                return false;
            }
            if (id == 0)
            {
                player.SendError("Account not found...");
                return false;
            }
            if (string.IsNullOrWhiteSpace(reason))
            {
                player.SendError("A reason must be provided.");
                return false;
            }
            var acc = db.GetAccount(id);
            if (string.IsNullOrEmpty(acc.IP))
            {
                player.SendError("Failed to ip ban player. IP not logged...");
                return false;
            }
            if (player.AccountId != acc.AccountId && acc.IP.Equals(player.Client.Account.IP))
            {
                player.SendError("IP ban failed. That action would cause yourself to be banned (IPs are the same).");
                return false;
            }
            if (player.AccountId != acc.AccountId && player.Rank <= acc.Rank)
            {
                player.SendError("Cannot ban players of equal or higher rank than yourself.");
                return false;
            }

            // ban
            db.Ban(acc.AccountId, reason);
            db.BanIp(acc.IP, reason);

            Program.DL.LogToDiscord(WebHooks.ModCommand, "Ban IP Log", $"Ban IP Log: [{player.Name}] has ip banned [{acc.Name}] for {reason}");

            // disconnect currently connected
            var targets = manager.Clients.Keys.Where(c => c.IP.Equals(acc.IP));
            foreach (var t in targets)
                t.Disconnect();

            // send notification
            player.SendInfo($"Banned {acc.Name} (both account and ip).");
            return true;
        }
    }

    internal class UnBanAccountCommand : Command
    {
        public UnBanAccountCommand() : base("unban", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string args)
        {
            var db = player.Manager.Database;

            // validate command
            var rgx = new Regex(@"^(\w+)$");
            if (!rgx.IsMatch(args))
            {
                player.SendError("Usage: /unban <account id or name>");
                return false;
            }

            // get info from args
            if (!int.TryParse(args, out int id))
                id = db.ResolveId(args);

            // run checks
            if (id == 0)
            {
                player.SendError("Account doesn't exist...");
                return false;
            }

            var acc = db.GetAccount(id);

            // unban
            var banned = db.UnBan(id);
            var ipBanned = acc.IP != null && db.UnBanIp(acc.IP);

            // send notification
            if (!banned && !ipBanned)
            {
                player.SendInfo($"{acc.Name} wasn't banned...");
                return true;
            }
            if (banned && ipBanned)
            {
                Program.DL.LogToDiscord(WebHooks.ModCommand, "Unban Log", $"Unban Log: [{player.Name}] has unbanned [{acc.Name}]");
                player.SendInfo($"Success! {acc.Name}'s account and IP no longer banned.");
                return true;
            }
            if (banned)
            {
                Program.DL.LogToDiscord(WebHooks.ModCommand, "Unban Log", $"Unban Log: [{player.Name}] has unbanned [{acc.Name}]");
                player.SendInfo($"Success! {acc.Name}'s account no longer banned.");
                return true;
            }
            Program.DL.LogToDiscord(WebHooks.ModCommand, "Unban Log", $"Unban Log: [{player.Name}] has unbanned [{acc.Name}]");
            player.SendInfo($"Success! {acc.Name}'s IP no longer banned.");
            return true;
        }
    }

    internal class HideCommand : Command
    {
        public HideCommand() : base("hide", Perms.Mod, alias: "h")
        {
        }

        protected override bool Process(Player player, string args)
        {
            var acc = player.Client.Account;

            acc.Hidden = !acc.Hidden;
            acc.FlushAsync();

            if (acc.Hidden)
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Hidden);
                player.ApplyConditionEffect(ConditionEffectIndex.Invincible);
                player.ApplyConditionEffect(ConditionEffectIndex.Swiftness);
                player.Manager.Clients[player.Client].Hidden = true;
            }
            else
            {
                player.ApplyConditionEffect(ConditionEffectIndex.Hidden, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Invincible, 0);
                player.ApplyConditionEffect(ConditionEffectIndex.Swiftness, 0);
                player.Manager.Clients[player.Client].Hidden = false;
            }
            return true;
        }
    }

    #endregion ModerationCommands

    #region UtilityCommands

    internal class VisitCommand : Command
    {
        public VisitCommand() : base("visit", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                player.SendInfo("Usage: /visit <player name>");
                return true;
            }

            var target = player.Manager.Clients.Keys
                .SingleOrDefault(c => c.Account != null &&
                   c.Account.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (target?.Player?.Owner == null ||
                !target.Player.CanBeSeenBy(player))
            {
                player.SendError("Player not found!");
                return false;
            }

            var owner = target.Player.Owner;
            player.Client.Reconnect(new Reconnect()
            {
                Host = "",
                GameId = owner.Id,
                Name = owner.SBName
            });
            return true;
        }
    }

    internal class WhereAllCommand : Command
    {
        public WhereAllCommand() : base("whereall", Perms.Mod)
        {
        }

        protected override bool Process(Player player, string args)
        {
            Dictionary<string, List<string>> locs = new Dictionary<string, List<string>>();
            var servers = player.Manager.InterServer.GetServerList();

            foreach (var server in servers)
            {
                if (!server.name.Equals(player.Manager.Config.serverInfo.name))
                    continue;
                foreach (PlayerInfo plr in server.playerList)
                {
                    if (plr.Hidden && !player.Client.Account.Admin || plr.WorldName == null)
                        continue;

                    if (locs.ContainsKey(plr.WorldName))
                        locs[plr.WorldName].Add(plr.Name);
                    else
                        locs.Add(plr.WorldName, new List<string> { plr.Name });
                }
                break;
            }

            string output = "Players online: ";

            foreach (var pair in locs)
            {
                output = output + "\n[ " + pair.Key + " ]: ";
                foreach (string name in pair.Value)
                    output = output + name + ", ";
                output = output.Substring(0, output.Length - 2);
            }

            player.SendInfo(output);
            return true;
        }
    }

    internal class FetchIDCommand : Command
    {
        public FetchIDCommand() : base("fetchid", Perms.Mod, alias: "getid")
        {
        }

        protected override bool Process(Player player, string args)
        {
            int id;
            try
            {
                id = player.Manager.Database.GetID(args); // ResolveId also works
            }
            catch
            {
                player.SendInfo("Format: /getid [playerName]");
                return false;
            }

            if (id > 0)
                player.SendInfo("Player '" + args + "' has the ID: " + id);
            else
            {
                player.SendError("Could not find ID associated with '" + args + "'");
                return false;
            }

            return true;
        }
    }

    internal class FetchUUIDCommand : Command
    {
        public FetchUUIDCommand() : base("fetchUUID", Perms.Owner, alias: "getuuid")
        {
        }

        protected override bool Process(Player player, string args)
        {
            DbAccount acc;
            try
            {
                int id = int.Parse(args);
                if (id < 1)
                {
                    player.SendInfo("Id cannot be less than 1!");
                    return false;
                }

                acc = player.Manager.Database.GetAccount(id); // ResolveIgn also works
                if (acc == null)
                {
                    player.SendInfo("Player not found, ID may be too large.");
                    return false;
                }
            }
            catch (Exception)
            {
                player.SendInfo("Format: /getuuid [playerID]");
                return false;
            }

            player.SendInfo(acc.UUID);

            return true;
        }
    }

    internal class FetchNameCommand : Command
    {
        public FetchNameCommand() : base("fetchname", Perms.Mod, alias: "getname")
        {
        }

        protected override bool Process(Player player, string args)
        {
            DbAccount acc;
            try
            {
                int id = int.Parse(args);
                if (id < 1)
                {
                    player.SendInfo("Id cannot be less than 1!");
                    return false;
                }

                acc = player.Manager.Database.GetAccount(id); // ResolveIgn also works
                if (acc == null)
                {
                    player.SendInfo("Player not found, ID may be too large.");
                    return false;
                }
            }
            catch (Exception)
            {
                player.SendInfo("Format: /getname [playerID]");
                return false;
            }

            player.SendInfo("Player with id '" + args + "' has the name: " + acc.Name);

            return true;
        }
    }

    #endregion UtilityCommands
}
