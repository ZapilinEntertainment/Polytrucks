using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class JoystickInput : SessionObject
	{		
		[SerializeField] private Joystick _joystick;
        private Transform _cameraTransform;
        private InputController _inputController;

        [Inject]
        public void Setup(PlayerController player, CameraController cameraController)
        {
            _inputController = player.InputController;
            _cameraTransform = cameraController.Camera.transform;
        }

        private void Start()
        {
            _joystick.gameObject.SetActive(true);
        }
        public void Update()
        {
            if (GameSessionActive)
            {
                var dir = _joystick.Direction;
                if (dir.sqrMagnitude != 0f)
                {
                    Vector3 projectedDir = Vector3.ProjectOnPlane(_cameraTransform.rotation * dir, Vector3.up).normalized;
                    _inputController.MoveCommand(new Vector2(projectedDir.x, projectedDir.z));
                }
            }
        }

        public override void OnSessionPause()
        {
            base.OnSessionPause();
            _joystick.enabled = false;
            _joystick.gameObject.SetActive(false);
        }
        public override void OnSessionResume()
        {
            base.OnSessionResume();
            _joystick.enabled = true;
            _joystick.gameObject.SetActive(true);
        }
    }
}
