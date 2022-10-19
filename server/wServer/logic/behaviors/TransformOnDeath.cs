using common.resources;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class TransformOnDeath : Behavior
    {
        private ushort target;
        private int min;
        private int max;
        private float probability;

        public TransformOnDeath(string target, int min = 1, int max = 1, double probability = 1)
        {
            this.target = GetObjType(target);
            this.min = min;
            this.max = max;
            this.probability = (float)probability;
        }

        protected internal override void Resolve(State parent)
        {
            parent.Death += (sender, e) =>
            {
                if (e.Host.CurrentState.Is(parent) &&
                    Random.NextDouble() < probability)
                {
                    if (Entity.Resolve(e.Host.Manager, target) is Portal
                        && e.Host.Owner.Name.Contains("Arena"))
                    {
                        return;
                    }
                    int count = Random.Next(min, max + 1);
                    for (int i = 0; i < count; i++)
                    {
                        Entity entity = Entity.Resolve(e.Host.Manager, target);
                        entity.Move(e.Host.X, e.Host.Y);

                        if (e.Host is Enemy && entity is Enemy && (e.Host as Enemy).Spawned)
                        {
                            (entity as Enemy).Spawned = true;
                        }

                        e.Host.Owner.EnterWorld(entity);
                    }
                }
            };
        }

        protected override void TickCore(Entity host, ref object state)
        { }
    }
}
