using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Filisha : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity filisha = Entity.Resolve(world.Manager, "Peepo");
            filisha.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(filisha);
        }
    }
}