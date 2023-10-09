using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {    
	public sealed class PlayerController : SessionObject
	{
        [SerializeField] private Vehicle _vehicle;
        [SerializeField] private InputController _inputController;
        public static Vector3 Position { get; private set; }
        public VirtualPoint FormVirtualPoint() => _vehicle.FormVirtualPoint();
        public InputController InputController => _inputController;
        

        private void Awake()
        {
            Position = transform.position;
            _inputController.Setup(this);
        }

        public override void OnSessionStart()
        {
            base.OnSessionStart();
            Transform pointLink = _vehicle.CameraViewPoint;
            _signalBus.Fire(new CameraViewPointSetSignal(pointLink));
        }

        #region controls
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
        #endregion
        private void LateUpdate()
        {
            if (GameSessionActive)  Position = _vehicle.Position;
        }

        public void Teleport(VirtualPoint point)
        {
            _vehicle.Teleport(point);
            Position = _vehicle.Position;
        }
    }
}
