using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private Cinemachine.CinemachineVirtualCamera _followCamera;
		public Camera Camera => _camera;

		[Inject]
		public void Setup(SignalBus signalBus)
		{
			signalBus.Subscribe<CameraViewPointSetSignal>(SetTrackPoint);
		}

		public void SetTrackPoint(CameraViewPointSetSignal args)
		{
			_followCamera.m_LookAt = args.Point;
			_followCamera.m_Follow = args.Point;
		}
	}
}
