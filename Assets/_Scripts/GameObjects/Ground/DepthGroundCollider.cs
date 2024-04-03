using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class DepthGroundCollider : GroundCollider
	{
        [SerializeField] protected float _squareSize = 1f;
        [SerializeField] protected GroundDepthSettings _depthSettings;
        protected Vector3 _center;

        protected override void Start()
        {
            _center = transform.position;
            base.Start();           
        }
        protected Vector2 GetInternalCoordinates(Vector3 hitpoint)
        {
            Vector3 inpos = transform.InverseTransformPoint(hitpoint); inpos.y = 0f;
            return new Vector2(
                inpos.x / _squareSize + 0.5f,
                inpos.z / _squareSize  + 0.5f
                );
        }

        protected override GroundCastInfo FormCastInfo(WheelCollisionInfo info)
        {
            Vector2 internalCoords = GetInternalCoordinates(info.Pos);
            return new GroundCastInfo(_passabilityParameters.Harshness,_passabilityParameters.Resistance,  _depthSettings.GetDepth(internalCoords));
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, _squareSize * Vector3.one);
        }
#endif
    }
}
