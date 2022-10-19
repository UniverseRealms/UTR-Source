using common.resources;
using wServer.networking;
using wServer.networking.packets.outgoing;
using wServer.realm.entities;
using wServer.realm.terrain;

namespace wServer.realm.worlds.logic
{
    internal class Nexus : World
    {
        public Nexus(ProtoWorld proto, Client client = null) : base(proto)
        {
        }
        private bool _created = false;
        protected override void Init()
        {
            base.Init();
            if (System.DateTime.UtcNow.Hour == 730 && !_created)
            {
                var worldboss = Entity.Resolve(Manager, "World Boss Portal");
                worldboss.Move(78f, 36f);
                EnterWorld(worldboss);
                _created = true;
            }
            
            var ca = Entity.Resolve(Manager, "Locked Champion's Arena Portal");
            ca.Move(79f, 50f);
            ca.Name = "Champion's Arena";
            EnterWorld(ca);

            var monitor = Manager.Monitor;
            foreach (var i in Manager.Worlds.Values)
            {
                if (i is Realm)
                {
                    monitor.AddPortal(i.Id);
                    continue;
                }

                if (i.Id >= 0)
                    continue;

              if (i is DeathArena)
                {
                    var portal = new Portal(Manager, 0x023D, null)
                    {
                        Name = "Champion's Arena",
                        WorldInstance = i
                    };

                    var pos = GetRegionPosition(TileRegion.Arena_Edge_Spawn);
                    if (pos == null)
                        continue;

                    monitor.AddPortal(i.Id, portal, pos);
                    continue;
                }

                if (i is Arena)
                {
                    var portal = new Portal(Manager, 0x7002, null)
                    {
                        Name = "Arena (0)",
                        WorldInstance = i
                    };

                    var pos = GetRegionPosition(TileRegion.Arena_Central_Spawn);
                    if (pos == null)
                        continue;

                    monitor.AddPortal(i.Id, portal, pos);
                    continue;
                }

                if (i is ArenaSolo)
                {
                    var portal = new Portal(Manager, 0x144, null)
                    {
                        Name = "Solo Arena",
                        WorldInstance = i
                    };

                    var pos = GetRegionPosition(TileRegion.Store_38);
                    if (pos == null)
                        continue;

                    monitor.AddPortal(i.Id, portal, pos);
                    continue;
                }

                if (i.Name.Equals("ClothBazaar"))
                {
                    var portal = new Portal(Manager, 0x167, null)
                    {
                        Name = "Cloth Bazaar (0)",
                        WorldInstance = i
                    };

                    var pos = GetRegionPosition(TileRegion.Store_39);
                    if (pos == null)
                        continue;

                    monitor.AddPortal(i.Id, portal, pos);
                    continue;
                }

                if (i is Marketplace && Manager.Config.serverSettings.enableMarket)
                {
                    var portal = new Portal(Manager, 0x69f7, null)
                    {
                        Name = "Marketplace (0)",
                        WorldInstance = i
                    };

                    var pos = GetRegionPosition(TileRegion.Store_37);
                    if (pos == null)
                        continue;

                    monitor.AddPortal(i.Id, portal, pos);
                }
            }
        }
    }
}