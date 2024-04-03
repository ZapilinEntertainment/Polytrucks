using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class TEST_Damper : MonoBehaviour
	{
		[SerializeField] private Transform _tireTransform, _wheelModel;
        [SerializeField] private float _suspensionLength = 4f, _springStrength = 10f, _springDamper = 1f, _wheelRadius = 0.5f;
        [SerializeField] private Rigidbody _rigidbody;

        private void FixedUpdate()
        {
            if (Physics.Raycast(_tireTransform.position, -_tireTransform.up, maxDistance: _suspensionLength + 1f, hitInfo: out var hit))
            {
                float offset =  _suspensionLength - hit.distance;
                Vector3 velocity = _rigidbody.GetPointVelocity(_tireTransform.position);
                float vel = Vector3.Dot(_tireTransform.up, velocity);

                float force = (offset * _springStrength) - (vel * _springDamper);
                _rigidbody.AddForceAtPosition(_tireTransform.up * force, _tireTransform.position);

                PositionWheel(hit.distance);
            }
            else
            {
                PositionWheel(_suspensionLength + 1f);
            }

            void PositionWheel(float y)
            {
                _wheelModel.position = _tireTransform.TransformPoint(Vector3.down * (y - _wheelRadius));
            }
        }
    }
}
