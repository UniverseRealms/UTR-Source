using common;
using System;
using System.Linq;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.worlds;

namespace wServer.realm.commands
{
    public abstract class DonorCommands
    {

        private static readonly Random Rand = new Random();

        #region Donor

        internal class GlowCommand : Command
        {
            public GlowCommand() : base("glow", Perms.Donor)
            {
            }

            protected override bool Process(Player player, string color)
            {
                if (string.IsNullOrWhiteSpace(color))
                {
                    player.SendInfo("Usage: /glow <color>");
                    return true;
                }

                player.Glow = Utils.FromString(color);

                var acc = player.Client.Account;
                acc.GlowColor = player.Glow;
                acc.FlushAsync();

                return true;
            }
        }
        

        internal class Level20Command : Command
        {
            public Level20Command(RealmManager manager) : base("level20", Perms.Donor, alias: "l20")
            {
                _manager = manager;
            }

            private readonly RealmManager _manager;

            protected override bool Process(Player player, string args)
            {
                if (player.Level < 20)
                {
                    var statInfo = _manager.Resources.GameData.Classes[player.ObjectType].Stats;
                    if (!player.AscensionEnabled)
                        for (var v = 0; v < statInfo.Length; v++)
                        {
                            player.Stats.Base[v] +=
                            (statInfo[v].MaxIncrease + statInfo[v].MinIncrease) * (21 - player.Level) / 2;
                            if (player.Stats.Base[v] > statInfo[v].MaxValue)
                                player.Stats.Base[v] = statInfo[v].MaxValue;
                        }
                    player.Level = 20;
                    return true;
                }
                return false;
            }
        }

        private class BazaarCommand : Command
        {
            public BazaarCommand() : base("bazaar", Perms.Donor)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.ClothBazaar,
                    Name = "Cloth Bazaar"
                });
                return true;
            }
        }

        private class DyeCommand : Command
        {
            public DyeCommand() : base("setdyeboth", Perms.Donor, alias: "dyeboth")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var gameData = player.Manager.Resources.GameData;

                string name = args.ToLower();

                // allow both DisplayId and Id for query
                if (!gameData.DisplayIdToObjectType.TryGetValue(args, out var objType))
                {
                    if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                    {
                        player.SendError("Unknown dye or cloth type!");
                        return false;
                    }
                }

                if (!gameData.Items.ContainsKey(objType))
                {
                    player.SendError("That isn't even an item!");
                    return false;
                }

                var item = gameData.Items[objType];
                var dyetype = Math.Max(item.Texture1,item.Texture2);
                if(dyetype == 0)
                {
                    player.SendError("That is not a valid dye or cloth!");
                    return false;
                }
                else
                {
                    player.Texture1 = dyetype;
                    player.Texture2 = dyetype;
                }           
                return true;
            }
        }

        private class DyeACommand : Command
        {
            public DyeACommand() : base("setdyea", Perms.Donor, alias: "dyea")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var gameData = player.Manager.Resources.GameData;

                string name = args.ToLower();

                // allow both DisplayId and Id for query
                if (!gameData.DisplayIdToObjectType.TryGetValue(args, out var objType))
                {
                    if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                    {
                        player.SendError("Unknown dye or cloth type!");
                        return false;
                    }
                }

                if (!gameData.Items.ContainsKey(objType))
                {
                    player.SendError("That isn't even an item!");
                    return false;
                }

                var item = gameData.Items[objType];
                var dyetype = Math.Max(item.Texture1, item.Texture2);
                if (dyetype == 0)
                {
                    player.SendError("That is not a valid dye or cloth!");
                    return false;
                }
                else
                {
                    player.Texture1 = dyetype;
                }
                return true;
            }
        }

        private class DyeBCommand : Command
        {
            public DyeBCommand() : base("setdyeb", Perms.Donor, alias: "dyeb")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var gameData = player.Manager.Resources.GameData;

                string name = args.ToLower();

                // allow both DisplayId and Id for query
                if (!gameData.DisplayIdToObjectType.TryGetValue(args, out var objType))
                {
                    if (!gameData.IdToObjectType.TryGetValue(args, out objType))
                    {
                        player.SendError("Unknown dye or cloth type!");
                        return false;
                    }
                }

                if (!gameData.Items.ContainsKey(objType))
                {
                    player.SendError("That isn't even an item!");
                    return false;
                }

                var item = gameData.Items[objType];
                var dyetype = Math.Max(item.Texture1, item.Texture2);
                if (dyetype == 0)
                {
                    player.SendError("That is not a valid dye or cloth!");
                    return false;
                }
                else
                {
                    player.Texture2 = dyetype;
                }
                return true;
            }
        }

        internal class RealmCommand : Command
        {
            public RealmCommand() : base("realm", 0)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.Realm,
                    Name = "Realm"
                });
                return true;
            }
        }

        internal class NexusCommand : Command
        {
            public NexusCommand() : base("nexus", Perms.Donor)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.Nexus,
                    Name = "Nexus"
                });
                return true;
            }
        }

        internal class VaultCommand : Command
        {
            public VaultCommand() : base("vault", Perms.Donor)
            {
            }

            protected override bool Process(Player player, string args)
            {
                player.Client.Reconnect(new Reconnect()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.Vault,
                    Name = "Vault"
                });
                return true;
            }
        }

        internal class SizeCommand : Command
        {
            public SizeCommand() : base("size", Perms.Donor)
            {
            }

            protected override bool Process(Player player, string args)
            {
                if (string.IsNullOrEmpty(args))
                {
                    player.SendError("Usage: /size <positive integer>. Using 0 will restore the default size for the sprite.");
                    return false;
                }

                var size = Utils.FromString(args);
                var min = player.Rank < 80 ? 75 : 0;
                var max = player.Rank < 80 ? 125 : 500;
                if (size < min && size != 0 || size > max)
                {
                    player.SendError($"Invalid size. Size needs to be within the range: {min}-{max}. Use 0 to reset size to default.");
                    return false;
                }

                var acc = player.Client.Account;
                acc.Size = size;
                acc.FlushAsync();

                var target = player.IsControlling ? player.SpectateTarget : player;
                if (size == 0)
                    target.RestoreDefaultSize();
                else
                    target.Size = size;

                return true;
            }
        }

        //internal class ReSkinCommand : Command
        //{
        //    public ReSkinCommand() : base("reskin", Perms.Donor)
        //    {
        //    }

        //    protected override bool Process(Player player, string args)
        //    {
        //        var skins = player.Manager.Resources.GameData.Skins
        //            .Where(d => d.Value.PlayerClassType == player.ObjectType)
        //            .Select(d => d.Key)
        //            .ToArray();

        //        if (String.IsNullOrEmpty(args))
        //        {
        //            var choices = skins.ToCommaSepString();
        //            player.SendError("Usage: /reskin <positive integer>");
        //            player.SendError("Choices: " + choices);
        //            return false;
        //        }

        //        var skin = (ushort)Utils.FromString(args);

        //        if (skin == 1025 || skin == 65303 || skin == 29786 || skin == 29788 || skin == 65296 || skin == 1026 || skin == 29787 || skin == 8614 || skin == 6122)
        //        {
        //            player.SendError("Error setting skin. Can't set skin to a Set Tier Skin.");
        //            return false;
        //        }
        //        if (skin != 0 && !skins.Contains(skin))
        //        {
        //            player.SendError("Error setting skin. Either the skin type doesn't exist or the skin is for another class.");
        //            return false;
        //        }

        //        var skinDesc = player.Manager.Resources.GameData.Skins[skin];
        //        var playerExclusive = skinDesc.PlayerExclusive;
        //        var size = skinDesc.Size;
        //        if (playerExclusive != null && !player.Name.Equals(playerExclusive))
        //        {
        //            skin = 0;
        //            size = 100;
        //        }

        //        player.SetDefaultSkin(skin);
        //        player.SetDefaultSize(size);
        //        return true;
        //    }
        //}

        #endregion Donor

        #region VIP
        internal class RouletteCommand : Command
        {
            public RouletteCommand() : base("Roulette", Perms.VIP, alias: "r")
            {
            }

            protected override bool Process(Player player, string args)
            {
                int randy = Rand.Next(0, 38);
                String sendString = "";

                if (randy == 0)
                    sendString = "Green 0";
                else if (randy == 37)
                    sendString = "Green 00";
                else if (randy == 1 || randy == 3 || randy == 5 || randy == 7 || randy == 9 || randy == 12 || randy == 14 || randy == 16 || randy == 18 || randy == 19 || randy == 21 || randy == 23 || randy == 25 || randy == 27 || randy == 30 || randy == 32 || randy == 34 || randy == 36)
                    sendString = "Red " + randy;
                else if (randy == 2 || randy == 4 || randy == 6 || randy == 8 || randy == 10 || randy == 11 || randy == 13 || randy == 15 || randy == 17 || randy == 20 || randy == 22 || randy == 24 || randy == 26 || randy == 28 || randy == 29 || randy == 31 || randy == 33 || randy == 35)
                    sendString = "Black " + randy;
                player.SendInfo(sendString);
                return true;
            }
        }

        internal class DrinkCommand : Command
        {
            public DrinkCommand() : base("Drink", Perms.Donor, alias: "d")
            {
            }

            protected override bool Process(Player player, string args)
            {
                var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];
                var acc = player.Client.Account;

                int[] DrinkAmount = new int[10];
                for(int i = 0; i <= 9; i++)
                DrinkAmount[0] = pd.Stats[0].MaxValue - player.Stats.Base[0];

                player.Stats.Base[0] += Math.Min(acc.StoredLife, DrinkAmount[0]);
                player.Stats.Base[1] += Math.Min(acc.StoredMana, DrinkAmount[1]);
                player.Stats.Base[2] += Math.Min(acc.StoredAttack, DrinkAmount[2]);
                player.Stats.Base[3] += Math.Min(acc.StoredDefense, DrinkAmount[3]);
                player.Stats.Base[4] += Math.Min(acc.StoredSpeed, DrinkAmount[4]);
                player.Stats.Base[5] += Math.Min(acc.StoredDexterity, DrinkAmount[5]);
                player.Stats.Base[6] += Math.Min(acc.StoredVitality, DrinkAmount[6]);
                player.Stats.Base[7] += Math.Min(acc.StoredWisdom, DrinkAmount[7]);
                player.Stats.Base[8] += Math.Min(acc.StoredLuck, DrinkAmount[8]);
                player.Stats.Base[9] += Math.Min(acc.StoredRestoration, DrinkAmount[9]);

                acc.StoredLife -= Math.Min(acc.StoredLife, DrinkAmount[0]);
                acc.StoredMana -= Math.Min(acc.StoredMana, DrinkAmount[1]);
                acc.StoredAttack -= Math.Min(acc.StoredAttack, DrinkAmount[2]);
                acc.StoredDefense -= Math.Min(acc.StoredDefense, DrinkAmount[3]);
                acc.StoredSpeed -= Math.Min(acc.StoredSpeed, DrinkAmount[4]);
                acc.StoredDexterity -= Math.Min(acc.StoredDexterity, DrinkAmount[5]);
                acc.StoredVitality -= Math.Min(acc.StoredVitality, DrinkAmount[6]);
                acc.StoredWisdom -= Math.Min(acc.StoredWisdom, DrinkAmount[7]);
                acc.StoredLuck -= Math.Min(acc.StoredLuck, DrinkAmount[8]);
                acc.StoredRestoration -= Math.Min(acc.StoredRestoration, DrinkAmount[9]);
                player.SendInfo("You have drunk some potions from storage");
                return true;
            }
        }

        internal class MaxCommand : Command
        {
            public MaxCommand() : base("max", Perms.Owner)
            {
            }

            protected override bool Process(Player player, string args)
            {
                var pd = player.Manager.Resources.GameData.Classes[player.ObjectType];

                player.Stats.Base[0] = pd.Stats[0].MaxValue;
                player.Stats.Base[1] = pd.Stats[1].MaxValue;
                player.Stats.Base[2] = pd.Stats[2].MaxValue;
                player.Stats.Base[3] = pd.Stats[3].MaxValue;
                player.Stats.Base[4] = pd.Stats[4].MaxValue;
                player.Stats.Base[5] = pd.Stats[5].MaxValue;
                player.Stats.Base[6] = pd.Stats[6].MaxValue;
                player.Stats.Base[7] = pd.Stats[7].MaxValue;
                player.Stats.Base[8] = pd.Stats[8].MaxValue;
                player.Stats.Base[9] = pd.Stats[9].MaxValue;
                player.SendInfo("Your character stats have been maxed.");
                return true;
            }
        }

        #endregion VIP
    }
}
