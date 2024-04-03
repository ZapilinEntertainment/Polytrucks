using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxleBase : MonoBehaviour
	{
        [SerializeField] protected Transform _leftWheel, _rightWheel;

        abstract public Vector3 Position { get; }
        abstract public Vector3 Forward { get; }

        abstract public void Setup(AxisControllerBase axisController);
        abstract public void Steer(float steerAngle);
    }
}
