using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum PlayerMoveStateType : byte { Idle, Gas, Brake, Reverse}
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

        private float _steerValue = 0f;
        private PlayerMoveStateType _currentPlayerMoveState = PlayerMoveStateType.Idle;
        private readonly PlayerController _player;
        private ControlsMask _controlsMask = new ControlsMask();

        public InputController (PlayerController player)
        {
            _player = player;           
        }

        public void MoveCommand(Vector2 dir)
        {
             _player.Move(dir);
        }
        public void StabilizeCommand()
        {
            _player.Stabilize();
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
            PlayerMoveStateType newPlayerMoveState;
            if (_controlsMask[ControlButtonID.Gas])
            {
                newPlayerMoveState = PlayerMoveStateType.Gas;
            }
            else
            {
                if (_controlsMask[ControlButtonID.Brake])
                {
                    newPlayerMoveState = PlayerMoveStateType.Brake;
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
            }
            if (_currentPlayerMoveState != newPlayerMoveState)
            {
                _currentPlayerMoveState = newPlayerMoveState;
                _player.ChangeMoveState(_currentPlayerMoveState);
            }

            float newSteer = 0f;
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
                _player.SetSteer(_steerValue);
            }
        }
    }
}
