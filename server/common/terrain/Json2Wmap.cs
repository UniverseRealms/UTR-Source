using common;
using common.resources;
using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace terrain
{
    internal class Json2Wmap
    {
        private struct obj
        {
            public string name;
            public string id;
        }

        private struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }

        private struct json_dat
        {
            public byte[] data;
            public int width;
            public int height;
            public loc[] dict;
        }

        public static void Convert(XmlData data, string from, string to)
        {
            var x = Convert(data, File.ReadAllText(from));
            File.WriteAllBytes(to, x);
        }

        public static byte[] Convert(XmlData data, string json)
        {
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);
            var tileDict = new Dictionary<short, TerrainTile>();

            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = obj.dict[i];
                var tileid = (ushort)0xff;

                try { if (o.ground != null && data.IdToTileType.ContainsKey(o.ground)) tileid = data.IdToTileType[o.ground]; }
                catch (Exception e) { XmlData.log.Error($"[Json Map Convert Exception]: unable to load tile type.\n- Id: {o.ground};\n- Exception: {e.ToString()}"); }

                var tileobj = o.objs?[0].id;
                var name = o.objs == null ? "" : o.objs[0].name ?? "";
                var terrain = TerrainType.None;
                var region = o.regions == null ? TileRegion.None : (TileRegion)Enum.Parse(typeof(TileRegion), o.regions[0].id.Replace(' ', '_'));
                var tt = new TerrainTile
                {
                    TileId = tileid,
                    TileObj = tileobj,
                    Name = name,
                    Terrain = terrain,
                    Region = region
                };

                tileDict[(short)i] = tt;
            }

            var tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[rdr.ReadInt16()];
                    }
            return WorldMapExporter.Export(tiles);
        }
    }
}
