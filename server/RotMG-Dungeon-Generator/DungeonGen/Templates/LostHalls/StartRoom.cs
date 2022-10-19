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
    internal class StartRoom : FixedRoom
    {
        internal Point portalPos;

        private static Range columnsNum = new Range(1, 4);

        public override RoomType Type { get { return RoomType.Start; } }

        public override int Width { get { return 17; } }

        public override int Height { get { return 17; } }

        static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.South, 5),
            Tuple.Create(Direction.North, 5),
            Tuple.Create(Direction.East, 5),
            Tuple.Create(Direction.West, 5)
        };

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override Range NumBranches { get { return columnsNum; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = LostHallsTemplate.LHMainTile
            });

            var buf = rasterizer.Bitmap;

            bool portalPlaced = false;
            while (!portalPlaced)
            {
                int x = (Bounds.X + Bounds.MaxX) / 2;
                int y = (Bounds.Y + Bounds.MaxY) / 2;

                buf[x, y].Region = "Spawn";
                buf[x, y].Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.CowardicePortal
                };
                portalPos = new Point(x, y);
                portalPlaced = true;
            }
        }
    }
}