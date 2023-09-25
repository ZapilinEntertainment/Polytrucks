using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public class SellZone : MonoBehaviour
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

        [SerializeField] private CollectiblesMask _collectiblesMask = new CollectiblesMask();
        private ColliderListSystem _collidersList;
        [Inject]
        public void Setup(ColliderListSystem collidersList)
        {
            _collidersList= collidersList;
        }


        public void Sell(ICollectable item)
        {

        }


        private void OnTriggerEnter(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller))
            {
                seller.TryStartSell(this, _collectiblesMask.Value);
            }
        }
    }
}
