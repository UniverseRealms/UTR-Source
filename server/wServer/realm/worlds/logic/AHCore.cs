using common.resources;
using System.Linq;
using wServer.networking;
using wServer.realm.entities;

namespace wServer.realm.worlds.logic
{
    internal class AHCore : World
    {
        private bool fin;

        public AHCore(ProtoWorld proto, Client client = null) : base(proto)
        {
        }

        public override void Tick()
        {
            if (Deleted) return;

            if (!fin)
            {
                if (!Enemies.Values.Any(e => (e.ObjectType == 0x23a2 || e.ObjectType == 0x6128))) { // AH The Heart
                    fin = true;
                    foreach (Player p in Players.Values) {
                        p.ApplyConditionEffect(ConditionEffectIndex.Damaging, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.Berserk, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.Bravery, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.Armored, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.Healing, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.Weak, 0);
                        //p.ApplyConditionEffect(ConditionEffectIndex.Dazed, 0);
                        p.ApplyConditionEffect(ConditionEffectIndex.ArmorBroken, 0);
                    }
                    return;
                }

                foreach (Player p in Players.Values) {
                    var tile = Map[(int)p.X, (int)p.Y];
                    switch (tile.TileId)
                    {
                        //"Algadrine Tile Heart", near beacons
                        case 0x266a:
                            if (tile.ObjId != 0)
                                break;
                            p.ApplyConditionEffect(ConditionEffectIndex.Damaging);
                            p.ApplyConditionEffect(ConditionEffectIndex.Berserk);
                            p.ApplyConditionEffect(ConditionEffectIndex.Bravery);
                            p.ApplyConditionEffect(ConditionEffectIndex.Armored);
                            p.ApplyConditionEffect(ConditionEffectIndex.Healing);
                            p.ApplyConditionEffect(ConditionEffectIndex.Weak, 0);
                            //p.ApplyConditionEffect(ConditionEffectIndex.Dazed, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.ArmorBroken, 0);
                            break;

                        default:
                            if (tile.ObjId != 0)
                                break;
                            p.ApplyConditionEffect(ConditionEffectIndex.Damaging, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.Berserk, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.Bravery, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.Armored, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.Healing, 0);
                            p.ApplyConditionEffect(ConditionEffectIndex.Weak);
                            //p.ApplyConditionEffect(ConditionEffectIndex.Dazed);
                            p.ApplyConditionEffect(ConditionEffectIndex.ArmorBroken);
                            break;
                    }
                }
            }
            
            base.Tick();
        }
    }
}