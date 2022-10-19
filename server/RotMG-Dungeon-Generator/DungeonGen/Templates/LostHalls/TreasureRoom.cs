using System;
using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.LostHalls
{
    internal class TreasureRoom : FixedRoom
    {
        public override RoomType Type { get { return RoomType.Special; } }

        public override int Width { get { return 17; } }

        public override int Height { get { return 17; } }

        static Random rand = new Random();
        static double randomDouble = rand.NextDouble();

        static readonly Tuple<Direction, int>[] connections =
        {
            Tuple.Create(Direction.South, 5),
            Tuple.Create(Direction.North, 5),
            Tuple.Create(Direction.East, 5),
            Tuple.Create(Direction.West, 5)
        };

        public override Range NumBranches { get { return new Range(1, 1); } }

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = LostHallsTemplate.LHMainTile
            });
            var buf = rasterizer.Bitmap;

            if (!LostHallsTemplate.oneSpecialRoom)
            {
                int x = (Bounds.X + Bounds.MaxX) / 2;
                int y = (Bounds.Y + Bounds.MaxY) / 2;

                buf[x, y].Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHAgonizedTitan
                };

                buf[x, y - 6].Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHCultPortalLocked
                };

                LostHallsTemplate.oneSpecialRoom = true;
            }
            else
            {

                for(int i = 0; i < 7; i++)
                {
                    int x = rand.Next(Bounds.X, Bounds.MaxX);
                    int y = rand.Next(Bounds.Y, Bounds.MaxY);

                    while (buf[x, y].Object != null)
                    {
                        x = rand.Next(Bounds.X, Bounds.MaxX);
                        y = rand.Next(Bounds.Y, Bounds.MaxY);
                    }

                    switch (i)
                    {
                        case 0:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot
                            };
                            break;
                        case 1:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot2
                            };
                            break;
                        case 2:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot3
                            };
                            break;
                        case 3:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot4
                            };
                            break;
                        case 4:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot5
                            };
                            break;
                        case 5:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot6
                            };
                            break;
                        case 6:
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = LostHallsTemplate.Pot7
                            };
                            break;
                    }
                }
            }
        }
    }
}