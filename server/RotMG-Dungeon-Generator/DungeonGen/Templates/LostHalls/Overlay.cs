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

using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Templates.LostHalls
{
	internal class Overlay : MapRender
    {
		void RenderWalls() {
			var wallA = new DungeonTile
            {
				TileType = LostHallsTemplate.Space,
				Object = new DungeonObject
                {
					ObjectType = LostHallsTemplate.LHMainWall
				}
			};
            var wallB = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHCandle1
                }
            };
            var wallC = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHCandle2
                }
            };
            var wallD = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHUnlitCandleWall
                }
            };
            var wallE = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHCrackedWall
                }
            };
            var wallF = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHTrapWall
                }
            };
            var wallG = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHVineWall
                }
            };
            var wallH = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHFaceWall1
                }
            };
            var wallI = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHFaceWall2
                }
            };
            var wallJ = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHWaterWall
                }
            };
            var wallK = new DungeonTile
            {
                TileType = LostHallsTemplate.Space,
                Object = new DungeonObject
                {
                    ObjectType = LostHallsTemplate.LHVoidWall
                }
            };

            var buf = Rasterizer.Bitmap;
            var tmp = (DungeonTile[,])buf.Clone();
            int w = Rasterizer.Width, h = Rasterizer.Height;

			for (int x = 0; x < w; x++)
				for (int y = 0; y < h; y++)
                {
					if (buf[x, y].TileType != LostHallsTemplate.Space)
						continue;

					bool notWall = true;
					if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
						notWall = true;
					else
                    {
						for (int dx = -1; dx <= 1 && notWall; dx++)
							for (int dy = -1; dy <= 1 && notWall; dy++) {
								if (tmp[x + dx, y + dy].TileType != LostHallsTemplate.Space)
                                {
									notWall = false;
									break;
								}
							}
					}
                    if (!notWall)
                    {
                        double rand = Rand.NextDouble();
                        buf[x, y] = rand < 0.025 ? wallK : rand < 0.05 ? wallJ : rand < 0.075 ? wallI : rand < 0.1 ? wallH : rand < 0.125 ? wallG : rand < 0.15 ? wallF : rand < 0.175 ? wallE : rand < 0.2 ? wallD : rand < 0.225 ? wallC : rand < 0.25 ? wallB : wallA;
                    }
				}
		}

        void RenderCustomTiles()
        {
            var buf = Rasterizer.Bitmap;
            int w = Rasterizer.Width, h = Rasterizer.Height;

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    if (buf[x, y].TileType == LostHallsTemplate.LHMainTile && Rand.NextDouble() < 0.05)
                        buf[x, y].TileType = LostHallsTemplate.LHMainTileBloody;
                    else if (buf[x, y].TileType == LostHallsTemplate.LHHallTile && Rand.NextDouble() < 0.05)
                        buf[x, y].TileType = LostHallsTemplate.LHHallTileBloody;
                        
        }

        void renderWallsOnSpawn()
        {
            var buf = Rasterizer.Bitmap;

            foreach (var room in Graph.Rooms)
            {
                if(room is StartRoom)
                {
                    int halfX = (room.Bounds.X + room.Bounds.MaxX) / 2;
                    int halfY = (room.Bounds.Y + room.Bounds.MaxY) / 2;

                    if (buf[halfX, room.Bounds.Y - 1].Object == null) // NORTH
                    {
                        for (int x = -3; x <= 3; x++)
                            buf[halfX + x, room.Bounds.Y - 1].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHBossWall };

                        buf[halfX, room.Bounds.Y + 1].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnPillar };
                        buf[halfX, room.Bounds.Y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnWallKiller };
                    }
                    if(buf[halfX, room.Bounds.MaxY].Object == null) //SOUTH
                    {
                        for (int x = -3; x <= 3; x++)
                            buf[halfX + x, room.Bounds.MaxY].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHBossWall };

                        buf[halfX, room.Bounds.MaxY - 2].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnPillar };
                        buf[halfX, room.Bounds.MaxY - 1].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnWallKiller };
                    }
                    if (buf[room.Bounds.X - 1, halfY].Object == null) //WEST
                    {
                        for (int y = -3; y <= 3; y++)
                            buf[room.Bounds.X - 1, halfY + y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHBossWall };

                        buf[room.Bounds.X + 1, halfY].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnPillar };
                        buf[room.Bounds.X, halfY].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnWallKiller };
                    }
                    if (buf[room.Bounds.MaxX, halfY].Object == null) //EAST
                    {
                        for (int y = -3; y <= 3; y++)
                            buf[room.Bounds.MaxX, halfY + y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHBossWall };

                        buf[room.Bounds.MaxX - 2, halfY].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnPillar };
                        buf[room.Bounds.MaxX - 1, halfY].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHSpawnWallKiller };
                    }
                    break;
                }
            }
        }

        void renderRandomRooms()
        {
            foreach (var room in Graph.Rooms)
            {
                if (room is NormalRoom)
                    randomRooms(room.Bounds, Rasterizer.Bitmap);
            }
        }

        void randomRooms(Rect bound, DungeonTile[,] buf)
        {
            double rand = Rand.NextDouble();
            #region Number1
            if (rand < 0.05)
            {
                double rand2 = Rand.NextDouble();
                if (rand2 < 0.25) //UPLEFT
                {
                    for (int x = bound.X; x < bound.X + 5; x++)
                        buf[x, bound.Y + 4].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                    for (int y = bound.Y; y < bound.Y + 5; y++)
                        buf[bound.X + 4, y].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                }
                else
                if (rand2 < 0.5) //UPRIGHT
                {
                    for (int x = bound.MaxX - 5; x < bound.MaxX; x++)
                        buf[x, bound.Y + 4].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                    for (int y = bound.Y; y < bound.Y + 5; y++)
                        buf[bound.MaxX - 5, y].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                }
                else
                if (rand2 < 0.75) //DOWNRIGHT
                {
                    for (int x = bound.MaxX - 5; x < bound.MaxX; x++)
                        buf[x, bound.MaxY - 5].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                    for (int y = bound.MaxY - 5; y < bound.MaxY; y++)
                        buf[bound.MaxX - 5, y].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                }
                else //DOWNLEFT
                {
                    for (int x = bound.X; x < bound.X + 5; x++)
                        buf[x, bound.MaxY - 5].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                    for (int y = bound.MaxY - 5; y < bound.MaxY; y++)
                        buf[bound.X + 4, y].Object = new DungeonObject
                        {
                            ObjectType = LostHallsTemplate.LHMarbleWall
                        };
                }
            }else
            #endregion
            #region Number2
            if (rand < 0.1)
            {
                for (int x = bound.X; x < bound.X + 5; x++)
                    buf[x, bound.Y + 4].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };
                for (int y = bound.Y; y < bound.Y + 5; y++)
                    buf[bound.X + 4, y].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };

                for (int x = bound.MaxX - 5; x < bound.MaxX; x++)
                    buf[x, bound.Y + 4].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };
                for (int y = bound.Y; y < bound.Y + 5; y++)
                    buf[bound.MaxX - 5, y].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };

                for (int x = bound.MaxX - 5; x < bound.MaxX; x++)
                    buf[x, bound.MaxY - 5].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };
                for (int y = bound.MaxY - 5; y < bound.MaxY; y++)
                    buf[bound.MaxX - 5, y].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };

                for (int x = bound.X; x < bound.X + 5; x++)
                    buf[x, bound.MaxY - 5].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };
                for (int y = bound.MaxY - 5; y < bound.MaxY; y++)
                    buf[bound.X + 4, y].Object = new DungeonObject
                    {
                        ObjectType = LostHallsTemplate.LHMarbleWall
                    };
            }else
            #endregion
            #region Number3
            if(rand < 0.15)
            {
                for (int x = bound.X + 2; x < bound.MaxX - 2; x = x + 3)
                    for (int y = bound.Y + 2; y < bound.MaxY - 2; y = y + 3)
                        if(buf[x, y].TileType != LostHallsTemplate.LHGemTile)
                            buf[x, y].TileType = LostHallsTemplate.LHHoleTile;

            }else
            #endregion
            #region Number4
            if(rand < 0.2)
            {
                double rand2 = Rand.NextDouble();
                for (int x = bound.X; x < bound.MaxX; x++)
                    for (int y = bound.Y; y < bound.MaxY; y++)
                        if (((x-bound.X) + (y-bound.Y) == 3 && rand2 <= 0.25) || ((bound.MaxX - x) + (bound.MaxY - y) == 5 && rand2 > 0.25 && rand <= 0.50) || ((bound.MaxX - x) + (y - bound.Y) == 4 && rand2 > 0.50 && rand2 <= 0.75) || ((x - bound.X) + (bound.MaxY - y) == 4 && rand2 > 0.75))
                            buf[x, y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHMarbleWall };
            }else
            #endregion
            #region Number5
            if(rand < 0.25)
            {
                for (int x = bound.X; x < bound.MaxX; x++)
                    for (int y = bound.Y; y < bound.MaxY; y++)
                        if ((x - bound.X) + (y - bound.Y) == 3 || (bound.MaxX - x) + (bound.MaxY - y) == 5 || (bound.MaxX - x) + (y - bound.Y) == 4 || (x - bound.X) + (bound.MaxY - y) == 4)
                            buf[x, y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHMarbleWall };
            }else
            #endregion
            #region Number6
            if(rand < 0.3)
            {
                double rand3 = Rand.NextDouble();

                for (int x = bound.X; x < bound.MaxX; x++)
                    for (int y = bound.Y; y < bound.MaxY; y++)
                    {
                        if (buf[x, y].TileType == LostHallsTemplate.LHGemTile)
                            continue;

                        double rand2 = Rand.NextDouble();
                        if(rand2 < 0.1)
                        {
                            if (rand3 < 0.5)
                                buf[x, y].TileType = LostHallsTemplate.LHHoleTile;
                            else
                                buf[x, y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHMarbleWall };
                        }
                    }
            }else
            #endregion
            #region Number7
            if(rand < 0.35)
            {
                double rand2 = Rand.NextDouble();
                for (int x = bound.X; x < bound.MaxX; x++)
                    for (int y = bound.Y; y < bound.MaxY; y++)
                        if((x - bound.X == 1 || x - bound.X == 2 || bound.MaxX - x == 2 || bound.MaxX - x == 3) && (y - bound.Y == 1 || y - bound.Y == 2 || bound.MaxY - y == 2 || bound.MaxY - y == 3))
                        {
                            if (rand2 < 0.25)
                                buf[x, y].TileType = LostHallsTemplate.LHHoleTile;
                            else if (rand2 < 0.5)
                                buf[x, y].TileType = LostHallsTemplate.LHWaterTile;
                            else if (rand2 < 0.75)
                                buf[x, y].TileType = LostHallsTemplate.LHIceTile;
                            else
                                buf[x, y].Object = new DungeonObject { ObjectType = LostHallsTemplate.LHMarbleWall };
                        }
            }
            #endregion
        }

        public override void Rasterize()
        {
			RenderWalls();
            renderRandomRooms();
            RenderCustomTiles();
            renderWallsOnSpawn();
		}
	}
}