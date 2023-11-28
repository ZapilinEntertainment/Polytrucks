using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class Elevator : TransportingPlatform
    {
        [Range(0f,1f)][SerializeField] private float _pathPositionPc = 0f;
        [SerializeField] private float _moveTime = 3f;
        [SerializeField] private Vector3 _startPoint, _endPoint;
        [SerializeField] private Transform _model;
        private bool _moveUp = false;

        private void Awake()
        {
            if (_pathPositionPc == 0f) _moveUp = true;
        }
        protected override bool TryReachDestination()
        {
            float target = _moveUp ? 1f : 0f;
            _pathPositionPc = Mathf.MoveTowards(_pathPositionPc, target, Time.deltaTime / _moveTime);
            _model.transform.position = transform.TransformPoint( Vector3.Lerp(_startPoint, _endPoint, _pathPositionPc));
            if (_pathPositionPc == target)
            {
                _moveUp = !_moveUp;
                return true;
            }
            else return false;
        }

        public void CallElevator(bool toStart)
        {
            if (_currentState.CanMove)
            {
                switch (_currentState.StateName)
                {
                    case PlatformState.Moving:
                        {
                            _moveUp = !toStart;
                            break;
                        }
                    case PlatformState.Ready:
                        {
                            if (toStart)
                            {
                                if (_pathPositionPc != 0f)
                                {
                                    _moveUp = false;
                                    ChangeState(PlatformState.Moving);
                                }
                            }
                            else
                            {
                                if (_pathPositionPc != 1f)
                                {
                                    _moveUp = true;
                                    ChangeState(PlatformState.Moving);
                                }
                            }
                            break;
                        }
                }
            }
        }

        override protected void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.DrawLine(transform.TransformPoint(_startPoint), transform.TransformPoint(_endPoint));
        }
    }
}
