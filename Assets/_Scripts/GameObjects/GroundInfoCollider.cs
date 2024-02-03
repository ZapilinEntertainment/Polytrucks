using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class GroundInfoCollider : MonoBehaviour
	{
        [SerializeField] private Collider _collider;
        [SerializeField] private GroundSettings _settings;
        private int _colliderID = -1;
        private float _xSize=1f, _zSize=1f;
        private EffectType _effectType = EffectType.Undefined;
        private Vector3 _center, _fwd, _right;
        private ColliderListSystem _colliderListSystem;
        private EffectsService _effectsService;
        public int GetColliderID() => _colliderID;

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem, EffectsService effectsService)
        {
            _colliderListSystem= colliderListSystem;
            _effectsService = effectsService;
        }

        private void Start()
        {
            _effectType = _settings.GroundType.GetMoveEffect();
            _colliderID = _collider.GetInstanceID();
            var bounds = _collider.bounds;
            _center = bounds.center;
            _fwd = transform.forward;
            _right = transform.right;
            _xSize = bounds.extents.x;
            _zSize = bounds.extents.z;

            _colliderListSystem.AddGroundInfo(this);            
        }

        public GroundCastInfo GetCastInfo(Vector3 pos, Vector3 forward)
        {
            Vector3 dir = pos - _center;
            float z = Vector3.Dot(_fwd, dir) / _zSize;
            float x = Vector3.Dot(_right, dir) / _xSize;

            if (_effectType != EffectType.Undefined && forward.sqrMagnitude > 1f) _effectsService.EmitEffect(_effectType, pos, -forward);

            return _settings.GetCastInfo(x, z);
        }
    }
}
