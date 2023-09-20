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
        public void ChangeMoveState(PlayerMoveStateType state)
        {
            switch (state)
            {
                case PlayerMoveStateType.Gas: _vehicle.Gas(); break;
                case PlayerMoveStateType.Brake: _vehicle.Brake(); break;
                case PlayerMoveStateType.Reverse: _vehicle.Reverse(); break;
                default: _vehicle.ReleaseGas(); break;
            }
        }
        public void SetSteer(float steer) => _vehicle.Steer(steer);
        private void LateUpdate()
        {
            if (GameSessionActive)  Position = _vehicle.Position;
        }
    }
}
