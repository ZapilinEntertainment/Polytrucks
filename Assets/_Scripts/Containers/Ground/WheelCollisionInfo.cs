using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public readonly struct WheelCollisionInfo
	{
        public readonly float WheelRadius;
        public readonly Vector3 Pos;
        public readonly Vector3 StepVector;        
        public float StepSqrLength => StepVector.sqrMagnitude;
        public float MoveStepLength => new Vector2(StepVector.x, StepVector.z).magnitude;
        public Vector3 Forward => StepVector.normalized;

        public WheelCollisionInfo(Vector3 pos)
        {
            Pos = pos;
            StepVector = Vector3.forward;
            WheelRadius = 1f;
        }
        public WheelCollisionInfo(Vector3 pos, Vector3 stepVector, float radius)
        {
            Pos = pos;
            StepVector = stepVector;
            WheelRadius = radius;
        }
    }
}
