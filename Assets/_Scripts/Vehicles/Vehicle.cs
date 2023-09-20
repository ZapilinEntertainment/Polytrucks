using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class Vehicle : SessionObject
	{
		[SerializeField] private Transform _cameraViewPoint;
		public abstract Vector3 Position { get; }
		public Transform CameraViewPoint => _cameraViewPoint;

		public abstract void Move(Vector2 dir);
        public abstract void Gas();
        public abstract void Brake();
        public abstract void Reverse();
        public abstract void ReleaseGas();
        public abstract void Steer(float x);
    }
}
