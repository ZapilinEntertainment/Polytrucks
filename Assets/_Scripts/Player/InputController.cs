using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum PlayerMoveStateType : byte { Idle, Gas, Reverse}
	public sealed class InputController
	{
        private class ControlsMask
        {
            private BitArray _controlsMask = new BitArray((int)ControlButtonID.Total, false);
            public bool this[ControlButtonID id]
            {
                get => _controlsMask[(int)id];
                set => _controlsMask[(int)id] = value;
            }
        }

        private bool _playerIsBraking = false, _areControlsLocked = false;
        private float _steerValue = 0f;
        private PlayerMoveStateType _currentPlayerMoveState = PlayerMoveStateType.Idle;
        private readonly IVehicleController _vehicleController;
        private ControlsMask _controlsMask = new ControlsMask();

        public InputController (PlayerController player)
        {
            _vehicleController = player.VehicleController;
            _areControlsLocked = _vehicleController.AreControlsLocked;
            _vehicleController.OnLoseControlsEvent += OnControlsLocked;
            _vehicleController.OnRestoreControlsEvent += OnControlsUnlocked;
        }
        private void OnControlsLocked()
        {
            _areControlsLocked = true;
            _steerValue = 0f;
            _playerIsBraking = false;
            _currentPlayerMoveState|= PlayerMoveStateType.Idle;
        }
        private void OnControlsUnlocked()
        {
            _areControlsLocked = false;
            Recalculation();
        }

        public void MoveCommand(Vector2 dir)
        {
             _vehicleController.Move(dir);
        }
        public void StabilizeCommand()
        {
            _vehicleController.Stabilize();
        }
        public void OnButtonDown(ControlButtonID button) {
            _controlsMask[button] = true;
            Recalculation();
        }
        public void OnButtonUp(ControlButtonID button) {
            _controlsMask[button] = false;
            Recalculation();
        }
        private void Recalculation()
        {
            if (_areControlsLocked) return;
            PlayerMoveStateType newPlayerMoveState;
            if (_controlsMask[ControlButtonID.Gas])
            {
                newPlayerMoveState = PlayerMoveStateType.Gas;
            }
            else
            {
                if (_controlsMask[ControlButtonID.Reverse])
                {
                    newPlayerMoveState = PlayerMoveStateType.Reverse;
                }
                else
                {
                    newPlayerMoveState = PlayerMoveStateType.Idle;
                }
            }
            if (_currentPlayerMoveState != newPlayerMoveState)
            {
                _currentPlayerMoveState = newPlayerMoveState;
                _vehicleController.ChangeMoveState(_currentPlayerMoveState);
            }

            float newSteer;
            if (_controlsMask[ControlButtonID.SteerLeft])
            {
                if (_controlsMask[ControlButtonID.SteerRight]) newSteer = 0f;
                else newSteer = -1f;
            }
            else
            {
                if (_controlsMask[ControlButtonID.SteerRight]) newSteer = 1f;
                else newSteer = 0f;
            }
            if (newSteer != _steerValue)
            {
                _steerValue = newSteer;
                _vehicleController.SetSteer(_steerValue);
            }

            bool braking = _controlsMask[ControlButtonID.Brake];
            if (braking != _playerIsBraking)
            {
                _playerIsBraking = braking;
                _vehicleController.SetBrake(_playerIsBraking);
            }
        }
    }
}
