using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public class UniversalSellZone : SellZoneBase
	{
        [Serializable]
        private class CollectiblesMask
        {
            [SerializeField] private CollectableType[] _collectiblesMask;
            private int _mask = -1;
            public int Value
            {
                get
                {
                    if (_mask == -1)
                    {
                        _mask = 0;
                        if (_collectiblesMask.Length > 0)
                        {
                            foreach (var type in _collectiblesMask)
                            {
                                _mask |= type.AsIntMaskValue();
                            }
                        }
                    }
                    return _mask;
                }
            }

            
        }

        [SerializeField] private RarityConditions _rarity = RarityConditions.Any;
        [SerializeField] private CollectiblesMask _collectiblesMask = new CollectiblesMask();

        public override TradeContract FormTradeContract() => new(_collectiblesMask.Value, FreeSlotsCount, _rarity);
    }
}
