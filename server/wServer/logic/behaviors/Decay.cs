﻿using wServer.realm;
using wServer.realm.cores;

namespace wServer.logic.behaviors
{
    internal class Decay : Behavior
    {
        //State storage: timer

        private int time;

        public Decay(int time = 10000)
        {
            this.time = time;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = this.time;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0) host.Owner.LeaveWorld(host);
            else cool -= (int)CoreConstant.worldLogicTickMs;

            state = cool;
        }
    }
}
