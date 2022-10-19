using common.resources;
using System.Linq;
using wServer.networking;
using wServer.logic;

namespace wServer.realm.worlds.logic
{
    class AHAuraController : World
    {
        private Entity TileControl;

        public AHAuraController(ProtoWorld proto, Client client = null) : base(proto)
        {
        }

        protected override void Init()
        {
            base.Init();

            if (Limbo) return;
            TileControl = Enemies.Values.SingleOrDefault(e => e.ObjectType == 0x6121); // "AH Aura Controller"
            
            if (TileControl != null)
            {
                TileControl.TickStateManually = true;
                TileControl.TickState(); // to prevent black screen bug when entering an AHAuraController world
                
                State fastState = State.FindState(TileControl.Manager.Behaviors.Definitions[TileControl.ObjectType].Item1, "DeactivateFast");
                
                if (!TileControl.CurrentState.Is(fastState))
                    TileControl.SwitchTo(fastState);
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (Limbo || Deleted || TileControl?.Manager == null || TileControl.Owner != this)
            {
                TileControl = null;
                return;
            }

            TileControl.TickState();
        }

        public override void LeaveWorld(Entity e)
        {
            if (e == TileControl)
            {
                TileControl = null;
            }

            base.LeaveWorld(e);
        }
    }
}
