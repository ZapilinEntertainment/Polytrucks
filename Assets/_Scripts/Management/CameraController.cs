using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CameraController : MonoBehaviour
	{
		[SerializeField] private float _fovChangeSpeed = 0.1f, _offsetChangeSpeed = 0.1f;
        [SerializeField] private Camera _camera;
		[SerializeField] private Cinemachine.CinemachineVirtualCamera _followCamera;
		private float _modifiedCameraValue = 0f, _defaultFov = 60f, _modifiedOffsetValue;
		private Vector3 _prevPoint, _defaultOffset;
		private Transform _cameraTransform, _targetPoint;
		private Cinemachine.CinemachineTransposer _transposer;
		public Camera Camera => _camera;
		private const float MAX_FOV = 90f, MAX_CAMERA_SPEED = 100f, MAX_OFFSET_FAR = 20f;

		[Inject]
		public void Inject(SignalBus signalBus)
		{
			signalBus.Subscribe<CameraViewPointSetSignal>(SetTrackPoint);
		}

        private void Awake()
        {
			_cameraTransform = _camera.transform;
			_prevPoint = _cameraTransform.position;
			_defaultFov = _followCamera.m_Lens.FieldOfView;

			_transposer = _followCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
			_defaultOffset = _transposer.m_FollowOffset;
        }

        public void SetTrackPoint(CameraViewPointSetSignal args)
		{
			_targetPoint = args.Point;
			_followCamera.m_LookAt = _targetPoint;
			_followCamera.m_Follow = _targetPoint;
		}
        private void Update()
        {
			if (_targetPoint == null) return;
			Vector3 currentPosition = _targetPoint.position;
			float speed = Vector3.Distance(_prevPoint, currentPosition), t = Time.deltaTime;
			float modifyTarget = 0f;
			if (speed > 0.1f)
			{
				modifyTarget = Mathf.Clamp01( (speed / t) / MAX_CAMERA_SPEED);
			}
			if (modifyTarget != _modifiedCameraValue)
			{
				_modifiedCameraValue = Mathf.MoveTowards(_modifiedCameraValue, modifyTarget, _fovChangeSpeed * t );
				_followCamera.m_Lens.FieldOfView = Mathf.Lerp(_defaultFov, MAX_FOV, _modifiedCameraValue);
				
			}
			if (modifyTarget != _modifiedOffsetValue)
			{
                _modifiedOffsetValue = Mathf.MoveTowards(_modifiedOffsetValue, modifyTarget, _offsetChangeSpeed * t );
                _transposer.m_FollowOffset = _defaultOffset + _modifiedOffsetValue * MAX_OFFSET_FAR * Vector3.ProjectOnPlane(_targetPoint.forward, Vector3.up).normalized;
            }           


            _prevPoint = currentPosition;
        }

    }
}
