using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class QueenofHearts : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity queen = Entity.Resolve(world.Manager, "Queen of Hearts");
            queen.Move(pos.X + 17.5f, pos.Y + 17.5f);
            world.EnterWorld(queen);
        }
    }
}