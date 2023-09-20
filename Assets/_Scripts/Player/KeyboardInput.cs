using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public sealed class KeyboardInput : SessionObject
    {
        private InputController _inputController;
        private float _previousHorizontal = 0f, _previousVertical = 0f;
        private BitArray _controlsMask = new BitArray((int)ControlButtonID.Total, false);

        protected override void OnAwake()
        {
            base.OnAwake();
            _inputController = SessionObjectsContainer.PlayerController.InputController;
            
        }
        public void Update()
        {
            if (GameSessionActive)
            {
                float horizontal = Input.GetAxis("Horizontal");
                if (horizontal != _previousHorizontal)
                {
                    CheckControl(ControlButtonID.SteerLeft, horizontal < 0f);
                    CheckControl(ControlButtonID.SteerRight, horizontal > 0f);
                    _previousHorizontal = horizontal;
                }
                float vertical = Input.GetAxis("Vertical");
                if (vertical != _previousVertical)
                {
                    CheckControl(ControlButtonID.Gas, vertical > 0f);
                    CheckControl(ControlButtonID.Reverse, vertical < 0f);
                    _previousVertical = vertical;
                }
                CheckControl(ControlButtonID.Brake, Input.GetKey(KeyCode.Space));
            }

            void CheckControl(ControlButtonID id, bool value)
            {
                bool buttonPressed = _controlsMask[(int)id];
                if (buttonPressed != value)
                {
                    if (buttonPressed) _inputController.OnButtonUp(id);
                    else _inputController.OnButtonDown(id);
                    _controlsMask[(int)id] = value;
                }
                
            }
        }

        public override void OnSessionPause()
        {
            base.OnSessionPause();
        }
        public override void OnSessionResume()
        {
            base.OnSessionResume();
        }
    }
}
