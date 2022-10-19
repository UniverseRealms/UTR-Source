using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Sanic : ISetPiece
    {
        public int Size
        {
            get { return 5; }
        }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity sanic = Entity.Resolve(world.Manager, "Sanic");
            sanic.Move(pos.X + 11.5f, pos.Y + 11.5f);
            world.EnterWorld(sanic);
        }
    }
}