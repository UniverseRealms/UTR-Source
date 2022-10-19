using common;
using System.Collections.Generic;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm.worlds.logic;

namespace wServer.realm.entities
{
    partial class Player
    {
        public enum GambleType
        {
            Unknown,
            Rock,
            Paper,
            Scissors
        }

        internal bool isGambling = false;
        internal Dictionary<Player, int> potentialGambler = new Dictionary<Player, int>();
        internal Player gambleTarget;
        internal GambleType gamble = GambleType.Unknown;
        internal int betAmount;

        public void RequestGamble(string name, int amount)
        {
            if (Owner is Test)
                return;

            Manager.Database.ReloadAccount(_client.Account);
            var acc = _client.Account;

            if (!acc.NameChosen)
            {
                SendError("You must choose a name before gambling.");
                return;
            }

            if (Owner.Name != "disabled")
            {
                SendError("Gambling is Disabled :)");
                return;
            }

            if (Database.GuestNames.Contains(name))
            {
                SendError("'" + name + "' needs to choose a unique name before they gamble.");
                return;
            }

            var target = Owner.GetUniqueNamedPlayer(name);
            if (target == null || !target.CanBeSeenBy(this))
            {
                SendError("'" + name + "' not found.");
                return;
            }

            if (target.Client.Account.AccountId == Client.Account.AccountId)
            {
                SendError("You can not gamble yourself.");
                return;
            }

            if (betAmount != target.betAmount)
            {
                SendError("You must have the same bet amount in order to gamble.");
                return;
            }

            if (target._client.Account.IgnoreList.Contains(AccountId))
                return; // account is ignored

            if (target.gambleTarget != null)
            {
                SendError("'" + target.Name + "' is already gambling.");
                return;
            }

            /*if (acc.Rank >= 30 || target.Client.Account.Rank >= 30)
            {
                SendError("One or both gamblers is rank 30 or above.");
                return;
            }*/

            if (potentialGambler.ContainsKey(target))
            {
                if (Credits >= amount && target.Credits >= amount)
                {
                    isGambling = true;
                    target.isGambling = true;

                    gambleTarget = target;
                    target.gambleTarget = this;

                    potentialGambler.Clear();
                    target.potentialGambler.Clear();

                    gamble = GambleType.Unknown;
                    target.gamble = GambleType.Unknown;

                    SendInfo("10 seconds to choose rock, paper or scissors. (To choose rock /pg r) (To choose paper /pg p) (To choose scissors /pg s)");
                    target.SendInfo("10 seconds to choose rock, paper or scissors. (To choose rock /pg r) (To choose paper /pg p) (To choose scissors /pg s)");
                }
                else
                {
                    SendInfo("A gambler doesn't have enough gold..tsk tsk.");
                    target.SendInfo("A gambler doesn't have enough gold..tsk tsk.");
                    isGambling = false;
                    target.isGambling = false;
                    gambleTarget = null;
                    target.gambleTarget = null;
                    return;
                }
                Owner.Timers.Add(new WorldTimer(10000, (w) =>
                {
                    if (w == null || w.Deleted || this == null || target == null)
                        return;

                    SendInfo("Time is up!");
                    target.SendInfo("Time is up!");
                    isGambling = false;
                    target.isGambling = false;
                    gambleTarget = null;
                    target.gambleTarget = null;

                    Player winner = null;
                    Player loser = null;

                    if (gamble != GambleType.Unknown && target.gamble != GambleType.Unknown)
                    {
                        if (gamble == GambleType.Paper && target.gamble == GambleType.Rock ||
                        gamble == GambleType.Rock && target.gamble == GambleType.Scissors ||
                        gamble == GambleType.Scissors && target.gamble == GambleType.Paper)
                        {
                            winner = this;
                            loser = target;
                        }
                        else if (gamble == GambleType.Rock && target.gamble == GambleType.Paper ||
                        gamble == GambleType.Scissors && target.gamble == GambleType.Rock ||
                        gamble == GambleType.Paper && target.gamble == GambleType.Scissors)
                        {
                            winner = target;
                            loser = this;
                        }
                        else if (gamble == target.gamble)
                        {
                            SendInfo($"The match was a tie with {gamble.ToString()}");
                            target.SendInfo($"The match was a tie with {gamble.ToString()}");
                            return;
                        }
                        else
                        {
                            SendInfo("Error.");
                            target.SendInfo("Error.");
                            return;
                        }

                        SendInfo($"{winner.Name} wins with {winner.gamble.ToString()}");
                        target.SendInfo($"{winner.Name} wins with {winner.gamble.ToString()}");

                        winner.Client.Manager.Database.UpdateCredit(winner.Client.Account, amount);
                        winner.Credits += amount;
                        winner.ForceUpdate(winner.Credits);

                        loser.Client.Manager.Database.UpdateCredit(loser.Client.Account, -amount);
                        loser.Credits -= amount;
                        loser.ForceUpdate(loser.Credits);

                        return;
                    }
                    else if (gamble == GambleType.Unknown && target.gamble == GambleType.Unknown)
                    {
                        SendInfo("Both gamblers stalled the match and therefore it's a tie!");
                        target.SendInfo("Both gamblers stalled the match and therefore it's a tie!");
                        return;
                    }
                    else
                    {
                        if (gamble == GambleType.Unknown && target.gamble != GambleType.Unknown)
                        {
                            winner = target;
                            loser = this;
                        }
                        else if (gamble != GambleType.Unknown && target.gamble == GambleType.Unknown)
                        {
                            winner = this;
                            loser = target;
                        }

                        SendInfo($"{loser.Name} stalled the match and therefore lost.");
                        target.SendInfo($"{loser.Name} stalled the match and therefore lost.");

                        winner.Client.Manager.Database.UpdateCredit(winner.Client.Account, amount);
                        winner.Credits += amount;
                        winner.ForceUpdate(winner.Credits);

                        loser.Client.Manager.Database.UpdateCredit(loser.Client.Account, -amount);
                        loser.Credits -= amount;
                        loser.ForceUpdate(loser.Credits);

                        return;
                    }
                }));
            }
            else
            {
                target.potentialGambler[this] = 1000 * 20;
                target._client.SendPacket(new GambleStart()
                {
                    Name = Name,
                    Amount = amount
                });
                SendInfo("You have sent a gamble request to " + name + " for " + amount + " gold.");
                return;
            }
        }
    }
}
