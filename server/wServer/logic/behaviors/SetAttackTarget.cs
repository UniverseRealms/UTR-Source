using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class SetAttackTarget : Behavior
    {

        private readonly ushort? _target;
        private readonly float _range;
        private readonly bool _seeInvis;
        private readonly bool _ally;

        public SetAttackTarget(string target, float range = 10, bool seeInvis = false)
        {
            _range = range;
            _target = Behavior.GetObjType(target);
            _seeInvis = seeInvis;
            if (_target != null && BehaviorDb.InitGameData.ObjectDescs[_target.Value].Ally)
                _ally = true;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int?)state ?? 500;

            if (cool <= 0) {
                if (_ally)
                    host.AttackTarget = host.GetNearestAlly(_range, _target, _seeInvis) ?? (host.AttackTarget?.Owner == null ? null : host.AttackTarget);
                else
                    host.AttackTarget = host.GetNearestEntity(_range, _target, _seeInvis) ?? (host.AttackTarget?.Owner == null ? null : host.AttackTarget);
                cool = 500;
            } else
                cool -= (int)CoreConstant.worldLogicTickMs;
            state = cool;
        }
    }
}