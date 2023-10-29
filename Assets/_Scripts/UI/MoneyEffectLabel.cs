using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class MoneyEffectLabel : MonoBehaviour, IPoolable
	{
        [SerializeField] private float _effectTime = 1f, _movePercent = 0.3f;
        [SerializeField] private TMPro.TMP_Text _label;
        [SerializeField] private RectTransform _rectTransform;
        private bool _isActive = false;
        private float _progress = 0f;
        private Vector3 _startPosition;
        private Color _cachedColor = Color.white;
        private Camera _camera;
        private MonoMemoryPool<MoneyEffectLabel> _pool;

        [Inject]
        public void Inject(CameraController cameraController)
        {
            _camera = cameraController.Camera;
        }
        public void Setup(int value, Color color, Vector3 startPos, Transform parent)
        {
            _label.text = value.ToString();
            _label.color = _cachedColor = color;
            _rectTransform.SetParent(parent);
            _startPosition = startPos;
            _rectTransform.position = _camera.WorldToScreenPoint(_startPosition);
            _rectTransform.localScale = Vector3.one;
            _progress = 0f;
            _isActive = true;
        }
        private void Update()
        {
            if (_isActive)
            {
                _progress = Mathf.MoveTowards(_progress, 1f, Time.deltaTime / _effectTime);
                transform.position = _camera.WorldToScreenPoint( _startPosition) + _progress * _movePercent * Screen.height * Vector3.up;
                _label.color = new Color(_cachedColor.r, _cachedColor.g, _cachedColor.b, 1f - _progress);
                if (_progress >= 1f)
                {                    
                    _pool.Despawn(this);
                }
            }
        }
        public void OnDespawned()
        {
            _isActive = false;
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
