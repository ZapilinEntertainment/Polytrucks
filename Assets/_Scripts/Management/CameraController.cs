using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private Cinemachine.CinemachineVirtualCamera _followCamera;
		public Camera Camera => _camera;

		public void SetTrackPoint(Transform t)
		{
			_followCamera.m_LookAt = t;
			_followCamera.m_Follow = t;
		}
	}
}
