using common.resources;
using System;
using wServer.networking.packets.outgoing;
using wServer.realm.terrain;

namespace wServer.realm.entities
{
    public partial class Player
    {
        private long l;

        private void HandleBastille()
        {
            try
            {
                if (RageBar > 0)
                {
                    ApplyConditionEffect(ConditionEffectIndex.Weak, 0);
                    ApplyConditionEffect(ConditionEffectIndex.Quiet, 0);
                }
                if (RageBar >= 90) ApplyConditionEffect(ConditionEffectIndex.Empowered, 2000);
                if (HasConditionEffect(ConditionEffects.Hidden)) return;
                if (Manager.Core.getTotalTickCount() - l <= 100 || Owner?.Name != "SummoningPoint") return;
                if (this.GetNearestEntity(150, 0x63ed) == null)
                {
                    this.GetNearestEntity(999, 0x63e7).ApplyConditionEffect(ConditionEffectIndex.Invulnerable);

                    if (RageBar == 0)
                    {
                        ApplyConditionEffect(ConditionEffectIndex.Weak);
                        ApplyConditionEffect(ConditionEffectIndex.Quiet);
                    }
                    else RageBar -= 2;
                }
                else
                {
                    this.GetNearestEntity(999, 0x63e7).ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 0);

                    if (RageBar < 100) RageBar += 1;
                    if (RageBar > 100) RageBar = 100;
                }

                l = Manager.Core.getTotalTickCount();
            }
            catch (Exception ex) { Program.Debug(typeof(Player), ex.ToString(), true); }
        }

        private void HandleUltraBastille()
        {
            try
            {
                if (RageBar > 0)
                {
                    ApplyConditionEffect(ConditionEffectIndex.Weak, 0);
                    ApplyConditionEffect(ConditionEffectIndex.Quiet, 0);
                }
                if (RageBar >= 90) ApplyConditionEffect(ConditionEffectIndex.Empowered, 2000);
                if (HasConditionEffect(ConditionEffects.Hidden)) return;
                if (Manager.Core.getTotalTickCount() - l <= 100 || Owner?.Name != "UltraSummoningPoint") return;
                if (this.GetNearestEntity(150, 0x63ed) == null)
                {
                    this.GetNearestEntity(999, 0x75f2).ApplyConditionEffect(ConditionEffectIndex.Invulnerable);

                    if (RageBar == 0)
                    {
                        ApplyConditionEffect(ConditionEffectIndex.Weak);
                        ApplyConditionEffect(ConditionEffectIndex.Quiet);
                    }
                    else RageBar -= 2;
                }
                else
                {
                    this.GetNearestEntity(999, 0x75f2).ApplyConditionEffect(ConditionEffectIndex.Invulnerable, 0);

                    if (RageBar < 100) RageBar += 1;
                    if (RageBar > 100) RageBar = 100;
                }

                l = Manager.Core.getTotalTickCount();
            }
            catch (Exception ex) { Program.Debug(typeof(Player), ex.ToString(), true); }
        }

        private bool HandleGround()
        {
            if (Manager.Core.getTotalTickCount() - l > 500)
            {
                if (HasConditionEffect(ConditionEffects.Paused) || HasConditionEffect(ConditionEffects.Invincible)) return false;

                var tile = Owner.Map[(int)X, (int)Y];

                if (CheckGroundDamage(this, tile)) return true;
            }
            return false;
        }

        public void ForceGroundHit(Position pos, int timeHit)
        {
            if (HasConditionEffect(ConditionEffects.Paused) || HasConditionEffect(ConditionEffects.Invulnerable) || HasConditionEffect(ConditionEffects.Invincible)) return;

            var tile = Owner.Map[(int)pos.X, (int)pos.Y];

            CheckGroundDamage(this, tile);
        }

        private static bool CheckGroundDamage(Player p, WmapTile tile)
        {
            var objDesc = tile.ObjType == 0 ? null : p.Manager.Resources.GameData.ObjectDescs[tile.ObjType];
            var tileDesc = p.Manager.Resources.GameData.Tiles[tile.TileId];

            if (tileDesc.Damaging && (objDesc == null || !objDesc.ProtectFromGroundDamage))
            {
                int dmg = (int)p.Client.Random.NextIntRange((uint)tileDesc.MinDamage, (uint)tileDesc.MaxDamage);
                dmg = dmg / 2;

                if (tileDesc.ZolTile)
                    p.HP =(int)(p.HP * 3 / 4);
                
                if (p.Protection > 0)
                {
                    if (dmg > p.Protection)
                    {
                        int overdmg = dmg - p.Protection;
                        p.HP -= overdmg;
                    }
                    p.Protection -= dmg;
                    p.Owner.BroadcastPacketNearby(new Damage()
                    {
                        TargetId = p.Id,
                        DamageAmount = (ushort)dmg,
                    }, p, p);
                    p.SetProtRegenCooldown();
                }
                else
                {
                    p.HP -= dmg;
                    p.Owner.BroadcastPacketNearby(new Damage()
                    {
                        TargetId = p.Id,
                        DamageAmount = (ushort)dmg,
                        Kill = p.HP <= 0,
                    }, p, p);
                }
                    
                if (p.HP <= 0)
                {
                    p.Death(tileDesc.ObjectId, tile: tile);
                    return true;
                }

                p.l = p.Manager.Core.getTotalTickCount();
            }
            return false;
        }
    }
}
