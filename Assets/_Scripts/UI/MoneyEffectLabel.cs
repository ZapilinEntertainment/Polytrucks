using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class MoneyEffectLabel : MonoBehaviour, IPoolable
	{
        private MonoMemoryPool<MoneyEffectLabel> _pool;

        public void Setup(int value, Color color)
        {

        }
        public void OnDespawned()
        {
            
        }

        public void OnSpawned()
        {
            
        }

        public class Pool : MonoMemoryPool<MoneyEffectLabel>
        {
            protected override void OnCreated(MoneyEffectLabel item)
            {
                base.OnCreated(item);
                item._pool = this;
            }
        }
    }
}
