using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace ZE.Polytrucks {
	public class ObjectScreenMarker : MonoBehaviour, IPoolable
	{
        [SerializeField] private Image _marker, _landMarker;
        private bool _isInScreen = true;
        private IWorldPositionable _target;
        private Canvas _canvas;
        private CameraController _camera;
        private MonoMemoryPool<ObjectScreenMarker> _pool;

        [Inject]
        public void Inject(UIManager uiManager, CameraController cameraController)
        {
            _canvas = uiManager.RootCanvas;
            _camera = cameraController;
        }

        public void OnDespawned()
        {
            _target = null;
        }

        public void OnSpawned() { }

        public void StartTracking(IWorldPositionable target, Color markerColor)
        {
            _target = target;
            _marker.color = markerColor;
            _landMarker.color = markerColor;
            gameObject.SetActive(true);
        }
        public void StopTracking() {
            if (_target != null)
            {
                _target = null;
                gameObject.SetActive(false);
                _pool.Despawn(this);
            }
        }

        private void Update()
        {
            Vector3 scrpos = _camera.WorldToScreenPoint(_target.GetWorldPosition());
            float scale = _canvas.scaleFactor;
            var imagePositioner = _marker.rectTransform;
            var imageRect = imagePositioner.rect;
            var screenRect = new Rect(0f, 0f, Screen.width - imageRect.width * scale, Screen.height - imageRect.height * scale);
            if (screenRect.Contains(scrpos))
            {
                if (!_isInScreen)
                {
                    _landMarker.enabled = true;
                    _isInScreen = true;
                }
            }
            else
            {
                scrpos.x = Mathf.Clamp(scrpos.x, 0f, screenRect.width);
                scrpos.y = Mathf.Clamp(scrpos.y, 0f, screenRect.height);
                if (_isInScreen)
                {
                    _landMarker.enabled = false;
                    _isInScreen = false;
                }
            }
            imagePositioner.position = scrpos;
        }



        public class Pool : MonoMemoryPool<ObjectScreenMarker>
        {
            protected override void OnCreated(ObjectScreenMarker item)
            {
                base.OnCreated(item);
                item._pool = this;
            }
        }
    }
}
