using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wServer.networking.packets;
using wServer.networking.packets.incoming;
using wServer.realm.entities;
using wServer.realm.worlds.logic;
using Player = wServer.realm.entities.Player;

namespace wServer.networking.handlers
{
    internal class UsePortalHandler : PacketHandlerBase<UsePortal>
    {
        private readonly int[] _realmPortals = { 0x0704, 0x070e, 0x071c, 0x703, 0x070d, 0x0d40 };

        public override PacketId ID => PacketId.USEPORTAL;

        protected override void HandlePacket(Client client, UsePortal packet)
        {
            //client.Manager.Logic.AddPendingAction(t => Handle(client, packet));
            Handle(client, packet);
        }

        private void Handle(Client client, UsePortal packet)
        {
            var player = client.Player;
            if (player?.Owner == null || IsTest(client))
                return;

            var entity = player.Owner.GetEntity(packet.ObjectId);
            if (entity == null) return;

            if (entity is GuildHallPortal portal)
            {
                HandleGuildPortal(player, portal);
                return;
            }

            HandlePortal(player, entity as Portal);
        }

        private static void HandleGuildPortal(Player player, GuildHallPortal portal)
        {
            if (portal.ObjectType == 0x072f)
            {
                var proto = player.Manager.Resources.Worlds["GuildHall"];
                var world = player.Manager.GetWorld(proto.id);
                player.Reconnect(world.GetInstance(player.Client));
                return;
            }
        }

        private void HandlePortal(Player player, Portal portal)
        {
            if (portal == null || !portal.Usable) return;
            if (Monitor.TryEnter(portal.CreateWorldLock, new TimeSpan(0, 0, 1)))
                try
                {
                    var world = portal.WorldInstance;

                    //if (player.Owner.Opener != player.Name && player.Credits < 1000 &&
                    //    (portal.ObjectType == 0x22c3 || portal.ObjectType == 0x63ae || portal.ObjectType == 0x612b || portal.ObjectType == 0x75b3)) {
                    //     player.SendError("You do not have enough gold to enter this raid!");
                    //      return;
                    // }

                    // special portal case lookup
                    if (world == null && _realmPortals.Contains(portal.ObjectType))
                    {
                        world = player.Manager.GetRandomGameWorld();
                        if (world == null)
                            return;
                    }

                    if (world is Realm && !player.Manager.Resources.GameData.ObjectTypeToId[portal.ObjectDesc.ObjectType].Contains("Cowardice"))
                    {
                        player.FameCounter.CompleteDungeon(player.Owner.Name);
                    }

                    if (world != null)
                    {
                        if (!world.canConnect && player.Rank < 70)
                        {
                            player.SendError("Dungeon is full.");
                            return;
                        }

                        if (player.Owner.Name.Equals("Tavern") && !player.AscensionEnabled && player.Rank < 70)
                        {
                            player.SendError("You must /ascend after having reached 2500 base fame and maxing 8/10 stats before you may enter portals here!");
                            return;
                        }
                        player.Reconnect(world);

                        if (portal.WorldInstance?.Invites != null)
                        {
                            portal.WorldInstance.Invites.Remove(player.Name.ToLower());
                        }
                        if (portal.WorldInstance?.Invited != null)
                        {
                            portal.WorldInstance.Invited.Add(player.Name.ToLower());
                        }
                        return;

                    }
                    // dynamic case lookup
                    if (portal.CreateWorldTask == null || portal.CreateWorldTask.IsCompleted)
                        portal.CreateWorldTask = Task.Factory
                            .StartNew(() => portal.CreateWorld(player))
                            .ContinueWith(e =>
                                Program.Debug(typeof(UsePortalHandler), e.Exception.ToString(), true),
                                TaskContinuationOptions.OnlyOnFaulted);

                    portal.WorldInstanceSet += player.Reconnect;
                }
                finally { Monitor.Exit(portal.CreateWorldLock); }
        }
    }
}
