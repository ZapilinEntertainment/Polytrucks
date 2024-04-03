using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class AppearingLabel : MonoBehaviour, IPoolable
	{
        [SerializeField] private TMPro.TMP_Text _label;        
        private MonoMemoryPool<AppearingLabel> _pool;       

        public void Setup(Vector3 screenPosition, string text, float fadeTime = 1f)
        {
            transform.position = screenPosition;
            _label.text = text;
            _label.color = Color.white;
            _label.DOFade(0f, fadeTime).SetDelay(1f).OnComplete(() => _pool.Despawn(this));
        }
        public void OnDespawned()
        {

        }

        public void OnSpawned()
        {
            
        }

        public class Pool : MonoMemoryPool<AppearingLabel>
        {
            protected override void OnCreated(AppearingLabel item)
            {
                base.OnCreated(item);
                item._pool = this;
            }
        }
    }
}
