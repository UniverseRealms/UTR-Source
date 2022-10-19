using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class Prioritize : Behavior
    {
        //State storage: none

        private CycleBehavior[] children;

        public Prioritize(params CycleBehavior[] children)
        {
            this.children = children;
        }

        protected override void OnStateEntry(Entity host, ref object state)
        {
            state = -1;
            foreach (var i in children)
                i.OnStateEntry(host);
        }

        protected override void TickCore(Entity host, ref object state)
        {
            int index;
            if (state == null) index = -1;
            else index = (int)state;

            if (index < 0)
            {
                index = 0;
                for (int i = 0; i < children.Length; i++)
                {
                    children[i].Tick(host);
                    if (children[i].Status == CycleStatus.InProgress)
                    {
                        index = i;
                        break;
                    }
                }
            }
            else
            {
                children[index].Tick(host);
                if (children[index].Status == CycleStatus.Completed ||
                    children[index].Status == CycleStatus.NotStarted)
                    index = -1;
            }

            state = index;
        }
    }
}
