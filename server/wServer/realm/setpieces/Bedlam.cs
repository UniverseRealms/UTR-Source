using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Bedlam : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity bedlam = Entity.Resolve(world.Manager, "Bedlamgod");
            bedlam.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(bedlam);
        }
    }
}