using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Burrower : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity hellfire = Entity.Resolve(world.Manager, "Transcendent Burrower");
            hellfire.Move(pos.X + 15.5f, pos.Y + 15.5f);
            world.EnterWorld(hellfire);
        }
    }
}