using System;
using wServer.realm;
using wServer.realm.terrain;

namespace wServer.logic.behaviors
{
    public class ChangeGroundOnDeath : Behavior
    {
        private readonly int dist;
        private readonly ushort[] groundToChange;
        private readonly ushort[] targetType;

        /// <summary>
        ///     Changes the ground if the monster dies
        /// </summary>
        /// <param name="GroundToChange">The tiles you want to change (null for every tile)</param>
        /// <param name="ChangeTo">The tiles who will replace the old once</param>
        /// <param name="dist">The distance around the monster</param>
        public ChangeGroundOnDeath(string[] GroundToChange, string[] ChangeTo, int dist)
        {
            if (GroundToChange != null) {
                groundToChange = new ushort[GroundToChange.Length];
                for (int i = 0; i < GroundToChange.Length; i++) {
                    ushort x = Program.Resources.GameData.IdToTileType[GroundToChange[i]];
                    groundToChange[i] = x;
                }
            }

            targetType = new ushort[ChangeTo.Length];
            for (int i = 0; i < ChangeTo.Length; i++) {
                ushort x = Program.Resources.GameData.IdToTileType[ChangeTo[i]];
                targetType[i] = x;
            }
            this.dist = dist;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                var w = e.Host.Owner;
                var pos = new IntPoint((int)e.Host.X - (dist / 2), (int)e.Host.Y - (dist / 2));
                if (w == null)
                    return;
                for (int x = Math.Max(0, pos.X); x < Math.Min(pos.X + dist, w.Map.Width); x++)
                {
                    for (int y = Math.Max(0, pos.Y); y < Math.Min(pos.Y + dist, w.Map.Height); y++)
                    {
                        WmapTile tile = w.Map[x, y];
                        if (groundToChange != null)
                        {
                            foreach (ushort type in groundToChange)
                            {
                                int r = Random.Next(targetType.Length);
                                if (tile.TileId == type)
                                {
                                    tile.TileId = targetType[r];
                                    tile.UpdateCount++;
                                }
                            }
                        }
                        else
                        {
                            int r = Random.Next(targetType.Length);
                            tile.TileId = targetType[r];
                            tile.UpdateCount++;
                        }
                    }
                }
            };
        }

        protected override void TickCore(Entity host, ref object state)
        {
        }
    }
}
