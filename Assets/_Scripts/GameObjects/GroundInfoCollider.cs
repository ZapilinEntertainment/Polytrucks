using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class GroundInfoCollider : MonoBehaviour
	{
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Collider _collider;
        [SerializeField] private GroundSettings _settings;
        private bool _useVisualDeformation = false, _materialHandlerActive = false;
        private int _colliderID = -1;
        private float _size=1f;        
        private EffectType _effectType = EffectType.Undefined;
        private Vector3 _center, _up;
        private Material _cachedMaterial;
        private GroundMaterialHandler _materialHandler;
        private ColliderListSystem _colliderListSystem;
        private EffectsService _effectsService;
        private GroundMaterialHandler.Pool _handlersPool;
        public int GetColliderID() => _colliderID;

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem, EffectsService effectsService, GroundMaterialHandler.Pool handlersPool)
        {
            _colliderListSystem= colliderListSystem;
            _effectsService = effectsService;
            _handlersPool= handlersPool;
        }

        private void Start()
        {
            _useVisualDeformation = _settings.UseVisualDeformation;
            _cachedMaterial = _renderer.sharedMaterial;
            _effectType = _settings.GroundType.GetMoveEffect();
            _colliderID = _collider.GetInstanceID();
            var bounds = _collider.bounds;
            _center = bounds.center;
            _up = transform.up;
            _size = bounds.size.x;
            float zsize = bounds.size.z;
            if (zsize > _size) _size = zsize;

            _colliderListSystem.AddGroundInfo(this);            
        }
        private void Update()
        {
            if (_useVisualDeformation && _materialHandlerActive)
            {
                if (_materialHandler.UpdateGround(Time.deltaTime))
                {
                    i_ReleaseMaterialHandler(true);
                }
            }
        }

        private Vector2 GetInternalCoordinates(Vector3 hitpoint)
        {
            Vector3 dir = transform.InverseTransformDirection(Vector3.ProjectOnPlane(hitpoint - _center, transform.up));
            return new Vector2(
                Vector3.Dot(Vector3.right, dir) / _size + 0.5f,
                Vector3.Dot(Vector3.forward, dir) / _size + 0.5f
                );
        }
        public GroundCastInfo OnWheelCollision(Vector3 pos, Vector3 forward, float wheelRadius)
        {
            if (_effectType != EffectType.Undefined && forward.sqrMagnitude > 1f) _effectsService.EmitEffect(_effectType, pos, -forward);
            Vector2 internalCoords = GetInternalCoordinates(pos);

            if (_useVisualDeformation)
            {
                if (_materialHandler == null)
                {
                    RequestMaterialHandler();
                    _materialHandler?.OnWheelTouched(internalCoords, wheelRadius / _size);
                }
                else _materialHandler.OnWheelTouched(internalCoords, wheelRadius / _size);
            }

            return _settings.GetCastInfo(internalCoords);
        }

        private void RequestMaterialHandler()
        {
            _materialHandler = _handlersPool.GetHandler();
            _materialHandlerActive = _materialHandler != null;
            if (_materialHandlerActive)
            {
                _materialHandler.StartHandling(_settings, _renderer);
                _materialHandler.OnDisposeEvent += ReleaseMaterialHandler;
            }
        }
        private void ReleaseMaterialHandler() => i_ReleaseMaterialHandler(false);
        private void i_ReleaseMaterialHandler(bool callFromInside)
        {
            if (_materialHandler != null)
            {
                _materialHandler.OnDisposeEvent -= ReleaseMaterialHandler;
                if (callFromInside) _materialHandler.Dispose();
                _materialHandler = null;                
            }
            _materialHandlerActive = false;
            _renderer.sharedMaterial = _cachedMaterial;
        }
    }
}
