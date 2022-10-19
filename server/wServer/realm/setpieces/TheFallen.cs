using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class TheFallen : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity fallen = Entity.Resolve(world.Manager, "TF The Fallen");
            fallen.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(fallen);
        }
    }
}