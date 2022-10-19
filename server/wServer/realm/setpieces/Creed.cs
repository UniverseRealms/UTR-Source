using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    internal class Creed : ISetPiece
    {
        public int Size { get { return 100; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["Creed"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}