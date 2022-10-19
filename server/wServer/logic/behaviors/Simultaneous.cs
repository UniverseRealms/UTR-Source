using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class Simultaneous : CycleBehavior
    {
        //State storage: none

        private CycleBehavior[] children;

        public Simultaneous(params CycleBehavior[] children)
        {
            this.children = children;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            foreach (var i in children)
                i.OnStateEntry(host);

            state = false;
        }

        protected override void TickCore(Entity host, ref object state)
        {
            bool reset = (bool?)state ?? false;
            Status = CycleStatus.InProgress;
            ushort finished = 0;

            for (int i = 0; i < children.Length; i++)
                if (children[i].Status != CycleStatus.Completed || reset)
                    children[i].Tick(host);
                else
                    finished++;

            reset = false;

            if (finished == children.Length)
            {
                Status = CycleStatus.Completed;
                reset = true;
            }

            state = reset;
        }
    }
}
