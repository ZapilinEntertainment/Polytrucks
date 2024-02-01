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
        private Vector3 _center, _fwd, _right;
        private ColliderListSystem _colliderListSystem;
        public int GetColliderID() => _colliderID;

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem)
        {
            _colliderListSystem= colliderListSystem;
        }

        private void Start()
        {
            _colliderID = _collider.GetInstanceID();
            var bounds = _collider.bounds;
            _center = bounds.center;
            _fwd = transform.forward;
            _right = transform.right;
            _xSize = bounds.extents.x;
            _zSize = bounds.extents.z;

            _colliderListSystem.AddGroundInfo(this);            
        }

        public GroundCastInfo GetCastInfo(Vector3 pos)
        {
            Vector3 dir = pos - _center;
            float z = Vector3.Dot(_fwd, dir) / _zSize;
            float x = Vector3.Dot(_right, dir) / _xSize;
            return _settings.GetCastInfo(x, z);
        }
    }
}
