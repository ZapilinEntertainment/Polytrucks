using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public  class VirtualCameraHandler : MonoBehaviour
	{
        [SerializeField] private bool _modifyOffset = true, _modifyFov = true;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _followCamera;
        [SerializeField] private CameraSettings _cameraSettings;

        private float _modifiedCameraValue = 0f, _defaultFov = 60f, _modifiedOffsetValue;        
        private Vector3 _prevPoint, _defaultOffset;
        private ICameraObservable _vehicleViewSettings = new CameraObservableSettings();
        private Transform _cameraTransform;
        private Transform _targetPoint;
        private Camera _camera;
        private Cinemachine.CinemachineTransposer _transposer;

        [Inject]
        public void Inject(CameraController cameraController)
        {
            _camera = cameraController.GetCamera();
            _cameraTransform = _camera.transform;
            _prevPoint = _cameraTransform.position;
        }

        private void Awake()
        {  
            _defaultFov = _followCamera.m_Lens.FieldOfView;
            _transposer = _followCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
            _defaultOffset = _transposer.m_FollowOffset;
        }
        public void SetTrackPoint(CameraViewPointSetSignal args)
        {
            _targetPoint = args.Point;
            _vehicleViewSettings = args.ViewSettings;
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
                modifyTarget = Mathf.Clamp01((speed / t) / _cameraSettings.MaxCameraSpeed);
            }
            if (modifyTarget != _modifiedCameraValue)
            {
                _modifiedCameraValue = Mathf.MoveTowards(_modifiedCameraValue, modifyTarget, _cameraSettings.FovChangeSpeed * t);
                _followCamera.m_Lens.FieldOfView = Mathf.Lerp(_defaultFov, _cameraSettings.MaxFov, _modifiedCameraValue);

            }
            if (modifyTarget != _modifiedOffsetValue)
            {
                _modifiedOffsetValue = Mathf.MoveTowards(_modifiedOffsetValue, modifyTarget, _cameraSettings.OffsetChangeSpeed * t);
                if (_modifyOffset) _transposer.m_FollowOffset = (_defaultOffset * _vehicleViewSettings.HeightViewCf + _modifiedOffsetValue * _cameraSettings.MaxOffsetY * Vector3.up * _vehicleViewSettings.HeightSpeedOffsetCf);
                //+ _modifiedOffsetValue * _cameraSettings.MaxOffsetZ * Vector3.ProjectOnPlane(_targetPoint.forward, Vector3.up).normalized
            }


            _prevPoint = currentPosition;
        }

        public Vector3 WorldToScreenPoint(Vector3 worldPoint) => _camera.WorldToScreenPoint(worldPoint);
    }
}
