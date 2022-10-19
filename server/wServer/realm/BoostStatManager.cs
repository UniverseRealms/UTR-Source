using common.resources;
using System.Linq;
using wServer.realm.entities;

namespace wServer.realm
{
    internal class BoostStatManager
    {
        private readonly StatsManager _parent;
        private readonly Player _player;
        private readonly SV<int>[] _boostSV;
        private readonly int[] _boost;

        public ActivateBoost[] ActivateBoost { get; }
        public int this[int index] => _boost[index];

        public BoostStatManager(StatsManager parent)
        {
            _parent = parent;
            _player = parent.Owner;
            _boost = new int[StatsManager.NUM_STAT_TYPES];

            _boostSV = new SV<int>[_boost.Length];
            for (var i = 0; i < _boostSV.Length; i++)
                _boostSV[i] = new SV<int>(_player, StatsManager.GetBoostStatType(i), _boost[i], i != 0 && i != 1);

            ActivateBoost = new ActivateBoost[_boost.Length];
            for (int i = 0; i < ActivateBoost.Length; i++)
                ActivateBoost[i] = new ActivateBoost();

            ReCalculateValues();
        }

        protected internal void ReCalculateValues(InventoryChangedEventArgs e = null)
        {
            for (var i = 0; i < _boost.Length; i++)
                _boost[i] = 0;

            ApplyEquipBonus(e);
            ApplySetBonus(e);
            ApplyActivateBonus(e);
            ApplyMarks(e);

            for (var i = 0; i < _boost.Length; i++)
                _boostSV[i].SetValue(_boost[i]);
        }

        private void ApplyEquipBonus(InventoryChangedEventArgs e)
        {
            for (var i = 0; i < 4; i++)
            {
                if (_player.Inventory[i] == null)
                    continue;

                foreach (var b in _player.Inventory[i].StatsBoost)
                    IncrementBoost((StatsType)b.Key, b.Value);

                foreach (var b in _player.Inventory[i].StatsBoostPerc)
                    if (b.Value != 0)
                    {
                        var index = StatsManager.GetStatIndex((StatsType)b.Key);
                        IncrementBoost((StatsType)b.Key, (int)((_parent.Base[index] + _boost[index]) * (b.Value / 100.0)));
                    }
            }
        }

        private void ApplySetBonus(InventoryChangedEventArgs e)
        {
            var gameData = _player.Manager.Resources.GameData;

            foreach (var equipSet in gameData.EquipmentSets.Values)
            {
                var setEquipped = equipSet.Setpieces.Where(piece => piece.Type.Equals("Equipment"))
                    .All(piece => (_player.Inventory[piece.Slot] == null && piece.ItemType == 0xFFFF) || 
                    (_player.Inventory[piece.Slot] != null && _player.Inventory[piece.Slot].ObjectType == piece.ItemType));

                if (setEquipped)
                {
                    // apply bonus
                    foreach (var ae in equipSet.ActivateOnEquipAll)
                    {
                        switch (ae.Effect)
                        {
                            case ActivateEffects.ChangeSkin:
                                _player.Skin = ae.SkinType;
                                _player.Size = ae.Size;
                                break;

                            case ActivateEffects.IncrementStat:
                                IncrementBoost((StatsType)ae.Stats, ae.Amount);
                                break;

                            case ActivateEffects.FixedStat:
                                FixedStat((StatsType)ae.Stats, ae.Amount);
                                break;

                            case ActivateEffects.ConditionEffectSelf:
                                _player.ApplyConditionEffect(ae.ConditionEffect.Value);
                                break;
                        }
                    }
                    return;
                }

                if (e == null)
                    continue;

                var setRemoved = equipSet.Setpieces.Where(piece => piece.Type.Equals("Equipment"))
                    .All(piece => (e.OldItems[piece.Slot] == null && piece.ItemType == 0xFFFF) || (e.OldItems[piece.Slot] != null && e.OldItems[piece.Slot].ObjectType == piece.ItemType));

                if (setRemoved)
                {
                    foreach (var ae in equipSet.ActivateOnEquipAll)
                    {
                        switch (ae.Effect)
                        {
                            case ActivateEffects.ChangeSkin:
                                _player.RestoreDefaultSkin();
                                _player.RestoreDefaultSize();
                                break;

                            case ActivateEffects.ConditionEffectSelf:
                                _player.ApplyConditionEffect(ae.ConditionEffect.Value, 0);
                                break;
                        }
                    }
                }
            }
        }

        private void ApplyActivateBonus(InventoryChangedEventArgs e)
        {
            for (var i = 0; i < ActivateBoost.Length; i++)
            {
                var b = ActivateBoost[i].GetBoost();
                _boost[i] += b;

                if (i > 7)
                    continue;

                var haveCondition = _player.HasConditionEffect((ConditionEffects)((ulong)1 << (i + 39)));
                if (b > 0)
                {
                    if (!haveCondition)
                        _player.ApplyConditionEffect(new ConditionEffect()
                        {
                            Effect = (ConditionEffectIndex)(i + 39),
                            DurationMS = -1
                        });
                }
                else
                {
                    if (haveCondition)
                        _player.ApplyConditionEffect(new ConditionEffect()
                        {
                            Effect = (ConditionEffectIndex)(i + 39),
                            DurationMS = 0
                        });
                }
            }
        }

        //private void ApplySacredEffects(InventoryChangedEventArgs e)
        //{
        //    if (_player.CheckSacredEffect(SacredEffects.EnhancedGrowth))
        //        IncrementBoost(StatsType.MaximumHP, (int)(GetStat(StatsType.MaximumHP) * _player.SacredBoost(SacredEffects.EnhancedGrowth, 0.05)));

        //    if (_player.CheckSacredEffect(SacredEffects.OpenMind))
        //        IncrementBoost(StatsType.MaximumMP, (int)(GetStat(StatsType.MaximumMP) * _player.SacredBoost(SacredEffects.OpenMind, 0.05)));

        //    if (_player.CheckSacredEffect(SacredEffects.CollectorsEdition))
        //    {
        //        double boost = (_player.SacredBoost(SacredEffects.CollectorsEdition, 0.0065)) * _player.Inventory.Where(inv => inv != null && inv.Sacred).Count();

        //        IncrementBoost(StatsType.MaximumHP, (int)(GetStat(StatsType.MaximumHP) * boost));
        //        IncrementBoost(StatsType.MaximumMP, (int)(GetStat(StatsType.MaximumMP) * boost));
        //        IncrementBoost(StatsType.Attack, (int)(GetStat(StatsType.Attack) * boost));
        //        IncrementBoost(StatsType.Defense, (int)(GetStat(StatsType.Defense) * boost));
        //        IncrementBoost(StatsType.Speed, (int)(GetStat(StatsType.Speed) * boost));
        //        IncrementBoost(StatsType.Dexterity, (int)(GetStat(StatsType.Dexterity) * boost));
        //        IncrementBoost(StatsType.Vitality, (int)(GetStat(StatsType.Vitality) * boost));
        //        IncrementBoost(StatsType.Wisdom, (int)(GetStat(StatsType.Wisdom) * boost));
        //        IncrementBoost(StatsType.Luck, (int)(GetStat(StatsType.Luck) * boost));
        //        IncrementBoost(StatsType.Restoration, (int)(GetStat(StatsType.Restoration) * boost));
        //    }

        //    if (_player.CheckBloodshed())
        //    {
        //        IncrementBoost(StatsType.Wisdom, (GetStat(StatsType.Defense) / 5));
        //    }

        //    if (_player.CheckOmni())
        //    {
        //        int count = 0;

        //        for(int i = 0; i <= 3; i++)
        //        {
        //            if (_player.Inventory[i] != null) 
        //                if(_player.Inventory[i].Legendary)
        //                count++;
        //        }

        //        double boost = 0.1 * count;
        //        if(count > 0)
        //        {
        //            IncrementBoost(StatsType.MaximumHP, (int)(GetStat(StatsType.MaximumHP) * boost));
        //            IncrementBoost(StatsType.MaximumMP, (int)(GetStat(StatsType.MaximumMP) * boost));
        //            IncrementBoost(StatsType.Attack, (int)(GetStat(StatsType.Attack) * boost));
        //            IncrementBoost(StatsType.Defense, (int)(GetStat(StatsType.Defense) * boost));
        //            IncrementBoost(StatsType.Speed, (int)(GetStat(StatsType.Speed) * boost));
        //            IncrementBoost(StatsType.Dexterity, (int)(GetStat(StatsType.Dexterity) * boost));
        //            IncrementBoost(StatsType.Vitality, (int)(GetStat(StatsType.Vitality) * boost));
        //            IncrementBoost(StatsType.Wisdom, (int)(GetStat(StatsType.Wisdom) * boost));
        //            IncrementBoost(StatsType.Luck, (int)(GetStat(StatsType.Luck) * boost));
        //            IncrementBoost(StatsType.Restoration, (int)(GetStat(StatsType.Restoration) * boost));
        //        }
        //    }
        //}

        private void ApplyMarks(InventoryChangedEventArgs e)
        {
            if (!_player.MarksEnabled) 
                return;

            ApplyNodeBoost(_player.Node1);
            ApplyNodeBoost(_player.Node2);
            ApplyNodeBoost(_player.Node3);
            ApplyNodeBoost(_player.Node4);
            ApplyMarkBoost();
        }

        private void ApplyNodeBoost(int node)
        {
            StatsType stat = StatsType.Speed;
            bool isSurge = false;

            switch (node)
            {
                case 1:
                    stat = StatsType.Defense;
                    break;
                case 2:
                    stat = StatsType.Vitality;
                    break;
                case 3:
                    stat = StatsType.Attack;
                    break;
                case 4:
                    stat = StatsType.Wisdom;
                    break;
                case 6:
                    stat = StatsType.Dexterity;
                    break;
                case 7:
                    stat = StatsType.Speed;
                    break;
                case 9:
                    stat = StatsType.Luck;
                    break;
                case 10:
                    isSurge = true;
                    break;
                case 11:
                    stat = StatsType.Restoration;
                    break;
            }

            if (isSurge) 
                return;

            IncrementBoost(stat, (int)(GetStat(stat) * 0.05));
        }

        private void ApplyMarkBoost()
        {
            const double DEFAULT_MPHP_INCREMENT = 0.25;
            const double DEFAULT_STAT_INCREMENT = 0.05;

            switch (_player.Mark)
            {
                case 1:
                    IncrementBoost(StatsType.MaximumMP, (int)(GetStat(StatsType.MaximumMP) * DEFAULT_MPHP_INCREMENT));
                    break;

                case 2:
                    IncrementBoost(StatsType.MaximumHP, (int)(GetStat(StatsType.MaximumHP) * DEFAULT_MPHP_INCREMENT));
                    break;
                case 6:
                    IncrementBoost(StatsType.Defense, (int)(GetStat(StatsType.Defense) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Vitality, (int)(GetStat(StatsType.Vitality) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Attack, (int)(GetStat(StatsType.Attack) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Wisdom, (int)(GetStat(StatsType.Wisdom) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Dexterity, (int)(GetStat(StatsType.Dexterity) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Speed, (int)(GetStat(StatsType.Speed) * DEFAULT_STAT_INCREMENT));
                    IncrementBoost(StatsType.Luck, (int)(GetStat(StatsType.Luck) * DEFAULT_STAT_INCREMENT));
                    break;
            }
        }

        private int GetStat(StatsType s)
        {
            var i = StatsManager.GetStatIndex(s);
            return _parent.Base[i] + _boost[i];
        }

        private void IncrementBoost(StatsType stat, int amount)
        {
            var i = StatsManager.GetStatIndex(stat);
            if (_parent.Base[i] + amount < 1)
                amount = (i == 0) ? -_parent.Base[i] + 2 : -_parent.Base[i];

            _boost[i] += amount;
        }

        private void FixedStat(StatsType stat, int value)
        {
            var i = StatsManager.GetStatIndex(stat);
            _boost[i] = value - _parent.Base[i];
        }
    }
}
