using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {    
	public sealed class PlayerController : SessionObject
	{
        [SerializeField] private Vehicle _vehicle;
        [SerializeField] private InputController _inputController;
        public static Vector3 Position { get; private set; }
        public InputController InputController => _inputController;

        protected override void OnAwake()
        {
            base.OnAwake();
            Position = transform.position;
        }

        public override void OnSessionStart()
        {
            base.OnSessionStart();
            SessionObjectsContainer.CameraController.SetTrackPoint(_vehicle.CameraViewPoint);
        }

        public void Move(Vector2 dir)
        {
            _vehicle.Move(dir);
        }
        private void LateUpdate()
        {
            if (GameSessionActive)  Position = _vehicle.Position;
        }
    }
}
