using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CameraController : MonoBehaviour
	{
		[SerializeField] private int _activeCameraVariant = 0;
        [SerializeField] private Camera _camera;
		[SerializeField] private VirtualCameraHandler[] _cameraVariants;
		private VirtualCameraHandler ActiveCameraHandler => _cameraVariants[_activeCameraVariant];
		public Camera GetCamera() => _camera;
		public new Transform transform => ActiveCameraHandler.transform;

		[Inject]
		public void Inject(SignalBus signalBus)
		{
			signalBus.Subscribe<CameraViewPointSetSignal>(SetTrackPoint);
		}

        private void Awake()
        {
            for(int i = 0; i < _cameraVariants.Length; i++)
			{
				_cameraVariants[i].gameObject.SetActive(i == _activeCameraVariant);
			}
        }

        public void SetTrackPoint(CameraViewPointSetSignal args)
		{
            ActiveCameraHandler.SetTrackPoint(args);
		}       

		public Vector3 WorldToScreenPoint(Vector3 worldPos) => ActiveCameraHandler.WorldToScreenPoint(worldPos);

    }
}
