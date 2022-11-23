using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoneTrucker
{
    public sealed class LT_InputModule : MonoBehaviour
    {
        private LT_PlayerController _playerController;
        private AcceleratorStatus _acceleratorStatus;
        private bool? _steerValue = null;
        private bool _isPaused = false;
        private void Start()
        {
            _playerController = FindObjectOfType<LT_PlayerController>();
        }

        public void SetPause(bool x)
        {
            _isPaused = x;
            if (x)
            {
                _acceleratorStatus = AcceleratorStatus.Idle;
                _steerValue = null;
                _playerController.ChangeAcceleratorStatus(AcceleratorStatus.Idle);
                _playerController.ChangeSteerValue(null);
            }
        }

        private void Update()
        {
            if (_isPaused) return;

            float x = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerController.ChangeAcceleratorStatus(AcceleratorStatus.BrakesDown);
            }
            else
            {
                x = Input.GetAxis("Vertical");
                if (x != 0f)
                {
                    if (x > 0f)
                    {
                        ChangeAcceleratorStatus(AcceleratorStatus.Accelerate);
                    }
                    else ChangeAcceleratorStatus(AcceleratorStatus.Backway);
                }
                else ChangeAcceleratorStatus(AcceleratorStatus.Idle);                
            }

            void ChangeAcceleratorStatus(AcceleratorStatus ast)
            {
                if (ast != _acceleratorStatus)
                {
                    _playerController.ChangeAcceleratorStatus(ast);
                    _acceleratorStatus = ast;
                }
            }

            x = Input.GetAxis("Horizontal");
            if (x != 0f)
            {
                if (x > 0f)
                {
                    if (_steerValue != true)
                    {
                        _playerController.ChangeSteerValue(true);
                        _steerValue = true;
                    }
                }
                else
                {
                    if (_steerValue != false)
                    {
                        _playerController.ChangeSteerValue(false);
                        _steerValue = false;
                    }
                }
            }
            else
            {
                if (_steerValue != null)
                {
                    _playerController.ChangeSteerValue(null);
                    _steerValue = null;
                }
            }

        }
    }
}
