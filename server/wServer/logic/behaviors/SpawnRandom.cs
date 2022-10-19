using common.resources;
using wServer.realm;
using wServer.realm.cores;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class SpawnRandom : Behavior
    {
        //State storage: SpawnRandom state
        private class SpawnRandomState
        {
            public Entity[] Spawned;
            public int RemainingTime;
        }

        private readonly int _spawnCount;
        private readonly ushort[] _children;
        private readonly int _maxChildren;
        private Cooldown _coolDown;
        private readonly int _coolDownOffset;
        private readonly bool _givesNoXp;

        public SpawnRandom(int spawnCount, string[] children, int maxChildren = 5, Cooldown coolDown = new Cooldown(), int coolDownOffset = 0, bool givesNoXp = true)
        {
            _spawnCount = spawnCount;
            _children = new ushort[children.Length];
            for (int i = 0; i < children.Length; i++)
                _children[i] = GetObjType(children[i]);
            _maxChildren = maxChildren;
            _coolDown = coolDown.Normalize();
            _coolDownOffset = coolDownOffset;
            _givesNoXp = givesNoXp;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = new SpawnRandomState()
            {
                Spawned = new Entity[_maxChildren],
                RemainingTime = _coolDownOffset
            };
        }

        protected override void TickCore(Entity host, ref object state)
        {
            SpawnRandomState spawn;
            if (state == null)
            {
                spawn = new SpawnRandomState {
                    Spawned = new Entity[_maxChildren],
                    RemainingTime = _coolDown.Next(Random)
                };
            }
            else
                spawn = state as SpawnRandomState;

            spawn.RemainingTime -= (int)CoreConstant.worldLogicTickMs;
            
            if (spawn.RemainingTime <= 0)
            {
                int r;
                int count = 0;
                for (int i = 0; i < spawn.Spawned.Length; i++) {
                    if (count >= _spawnCount) break;
                    if (spawn.Spawned[i]?.Owner != null) continue;

                    r = Random.Next(0, _children.Length);
                    spawn.Spawned[i] = SpawnEntity(host, _children[r], _givesNoXp);
                    count++;
                }
                spawn.RemainingTime = _coolDown.Next(Random);
            }
        }

        private static Entity SpawnEntity(Entity parent, ushort child, bool givesNoXp) {
            Entity entity = Entity.Resolve(parent.Manager, child);
            entity.Move(parent.X, parent.Y);

            var enemyHost = parent as Enemy;
            var enemyEntity = entity as Enemy;
            if (enemyHost != null && enemyEntity != null)
            {
                enemyEntity.Terrain = enemyHost.Terrain;
                enemyEntity.GivesNoXp = givesNoXp;
                if (enemyHost.Spawned)
                {
                    enemyEntity.Spawned = true;
                    enemyEntity.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Invisible,
                        DurationMS = -1
                    });
                }
            }

            parent.Owner.EnterWorld(entity);
            return entity;
        }
    }
}
