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
				
		public Camera Camera => _camera;
		

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
			_cameraVariants[_activeCameraVariant].SetTrackPoint(args);
		}       

    }
}
