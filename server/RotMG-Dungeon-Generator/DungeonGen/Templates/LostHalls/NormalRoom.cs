/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.LostHalls
{
    internal class NormalRoom : FixedRoom
    {
        readonly int w;
        readonly int h;

        public NormalRoom(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public override RoomType Type { get { return RoomType.Normal; } }

        public override int Width { get { return w; } }

        public override int Height { get { return h; } }

        static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.South, 5),
            Tuple.Create(Direction.North, 5),
            Tuple.Create(Direction.East, 5),
            Tuple.Create(Direction.West, 5)
        };

        public override Range NumBranches { get { return new Range(2, 3); } }

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = LostHallsTemplate.LHMainTile
            });

            var bounds = Bounds;
            var buf = rasterizer.Bitmap;

            buf[(bounds.X + bounds.MaxX) / 2, (Bounds.Y + bounds.MaxY) / 2].TileType = LostHallsTemplate.LHGemTile;

            double r = rand.NextDouble();
            var num = r < 0.25 ? 0 : r < 0.5 ? 1 : r < 0.75 ? 2 : 3;

            var CommanderNum = num == 0 ? 1 : 0;

            var ChampionOfOryxNum = num == 1 ? 1 : 0;

            var GrottoNum = num == 2 ? 1 : 0;

            var GolemNum = num == 3 ? 1 : 0;

            var RemainsNum = rand.Next(0, 4);

            while(CommanderNum > 0 || ChampionOfOryxNum > 0 || GrottoNum > 0 || GolemNum > 0 || RemainsNum > 0)
            {
                int x = rand.Next(bounds.X, bounds.MaxX);
                int y = rand.Next(bounds.Y, bounds.MaxY);

                if (buf[x, y].Object != null)
                    continue;

                switch(rand.Next(5))
                {
                    case 0:
                        if(CommanderNum > 0)
                        {
                            buf[(bounds.X + bounds.MaxX) / 2, (bounds.Y + bounds.MaxY) / 2].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Commander
                            };
                            CommanderNum--;
                        }
                        break;
                    case 1:
                        if (ChampionOfOryxNum > 0)
                        {
                            buf[(bounds.X + bounds.MaxX)/2, (bounds.Y + bounds.MaxY) / 2].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.ChampionOfOryx
                            };
                            ChampionOfOryxNum--;
                        }
                        break;
                    case 2:
                        if (GolemNum > 0)
                        {
                            buf[(bounds.X + bounds.MaxX) / 2, (bounds.Y + bounds.MaxY) / 2].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.TormentedGolem
                            };
                            GolemNum--;
                        }
                        break;
                    case 3:
                        if (GrottoNum > 0)
                        {
                            buf[(bounds.X + bounds.MaxX) / 2, (bounds.Y + bounds.MaxY) / 2].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.GrottoBlob
                            };
                            GrottoNum--;
                        }
                        break;
                    case 4:
                        if(RemainsNum > 0)
                        {
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.LHRemains
                            };
                            RemainsNum--;
                        }
                        break;
                }
            }
        }
    }
}