using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Blaze : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity blaze = Entity.Resolve(world.Manager, "Blaze-Born");
            blaze.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(blaze);
        }
    }
}