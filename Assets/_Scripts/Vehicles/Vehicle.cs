using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class Vehicle : MonoBehaviour
	{
		[SerializeField] private Transform _cameraViewPoint;
		public abstract Vector3 Position { get; }
		public Transform CameraViewPoint => _cameraViewPoint;
		public abstract void Move(Vector2 dir);
	}
}
