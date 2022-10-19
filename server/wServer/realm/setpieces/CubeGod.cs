using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class CubeGod : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }
        
        private byte[,] SetPiece
        {
            get
            {
                return new byte[,]
                {
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0},
                };
            }
        }
        
        

          public void RenderSetPiece(World world, IntPoint pos)
          {
                var dat = world.Manager.Resources.GameData;

                IntPoint p = new IntPoint
                {
                    X = pos.X - (Size / 2),
                    Y = pos.Y - (Size / 2)
                };

            for (int x = 0; x < Size; x++)
               {
                    for (int y = 0; y < Size; y++)
                    {
                         if (SetPiece[y, x] == 0)
                         {
                         var tile = world.Map[x + p.X, y + p.Y].Clone();
                         tile.TileId = dat.IdToTileType["Grey Quad"];
                         tile.ObjType = 0;
                         world.Map[x + p.X, y + p.Y] = tile;
                         }
                    }
               }

               Entity cube = Entity.Resolve(world.Manager, "Cube God");
               cube.Move(pos.X + 2.5f, pos.Y + 2.5f);
               world.EnterWorld(cube);


          }
    }
}
