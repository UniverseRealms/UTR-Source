using System;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    internal class Suicide : Behavior
    {
        //State storage: timer

        public Suicide()
        {
        }

        protected override void TickCore(Entity host, ref object state)
        {
            if (!(host is Enemy))
            {
                if (!(host is Ally))
                    throw new NotSupportedException("Use Decay instead");
                else
                    (host as Ally).Death();
            }
            else
                (host as Enemy).Death();
        }
    }
}
