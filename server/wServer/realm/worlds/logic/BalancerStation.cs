using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.resources;
using System.IO;
using terrain;
using wServer.networking;

namespace wServer.realm.worlds.logic
{
    class BalancerStation : Test
    {
        public BalancerStation(ProtoWorld proto, Client client = null) : base(proto)
        {
            JsonLoaded = true;
        }

        protected override void Init()
        {
            if (Limbo) return;

            var proto = Manager.Resources.Worlds[Name];

            if (proto.maps != null && proto.maps.Length <= 0)
            {
                var template = DungeonTemplates.GetTemplate(Name);
                if (template == null)
                    throw new KeyNotFoundException($"Template for {Name} not found.");
                FromDungeonGen(Rand.Next(), template);
                return;
            }

            var map = Rand.Next(0, proto.maps?.Length ?? 1);
            FromWorldMap(new MemoryStream(proto.wmap[map]));

            InitShops();
        }
    }
}
