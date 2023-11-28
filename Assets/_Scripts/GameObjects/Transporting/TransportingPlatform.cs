using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

    public enum PlatformState : byte { Ready, Blocked, Moving, Disabled}

	public abstract class TransportingPlatform : MonoBehaviour
	{
        #region states
        protected abstract class PlatformStateClass
        {
            protected TransportingPlatform _platform;
            abstract public bool CanMove { get; }
            abstract public PlatformState StateName { get; }
            abstract public PlatformState VisualState { get; }
            abstract public bool InnerTriggerActive { get; }

            public PlatformStateClass(TransportingPlatform platform)
            {
                _platform = platform;
            }

            public virtual void OnStateStart() { }
            public virtual void Update() { }
            public virtual void OnStateEnd() { }
            public virtual void OnPlayerEnter() { }
            public virtual void OnPlayerExit() { }
        }
        protected class ReadyState : PlatformStateClass
        {
            public override bool CanMove => true;
            public override PlatformState StateName => PlatformState.Ready;
            public override PlatformState VisualState => PlatformState.Ready;
            public override bool InnerTriggerActive => true;
            public ReadyState(TransportingPlatform platform) : base(platform) { }
            public override void OnPlayerEnter()
            {
                _platform.ChangeState(PlatformState.Blocked);
            }
        }
        protected class BlockedState : PlatformStateClass
        {
            public override bool CanMove => false;
            public override PlatformState StateName => PlatformState.Blocked;
            public override PlatformState VisualState => PlatformState.Blocked;
            public override bool InnerTriggerActive => true;
            public BlockedState(TransportingPlatform platform) : base(platform) { }

            public override void OnStateStart()
            {
                _platform.SetRendererState(PlatformState.Blocked);
                _platform._playerTrigger.SetActivity(true);
            }
            public override void Update()
            {
                if (_platform.CanStartMovement())
                {
                    _platform.ChangeState(PlatformState.Moving);
                }
            }
            public override void OnPlayerExit()
            {
                _platform.ChangeState(PlatformState.Ready);
            }
        }
        protected class DisabledState : PlatformStateClass
        {
            public override bool CanMove => false;
            public override PlatformState StateName => PlatformState.Disabled;
            public override PlatformState VisualState => PlatformState.Disabled;
            public override bool InnerTriggerActive => false;
            public DisabledState(TransportingPlatform platform) : base(platform) { }
            public override void OnStateStart()
            {
                _platform.SetRendererState(PlatformState.Disabled);
                _platform._playerTrigger.SetActivity(false);
            }
        }
        protected class MovingState : PlatformStateClass
        {
            public override bool CanMove => true;
            public override PlatformState StateName => PlatformState.Moving;
            public override PlatformState VisualState => PlatformState.Moving;
            public override bool InnerTriggerActive => true;
            public MovingState(TransportingPlatform platform) : base(platform) { }

            public override void OnStateStart()
            {
                _platform.SetRendererState(PlatformState.Moving);
                _platform._playerTrigger.SetActivity(false);
            }
            public override void Update()
            {
                if (_platform.TryReachDestination())
                {
                    if (_platform._playerTrigger.IsPlayerInside) _platform.ChangeState(PlatformState.Blocked);
                    else _platform.ChangeState(PlatformState.Ready);                    
                }
            }
            public override void OnStateEnd()
            {
                _platform.UnlockPlayer();
                if (_platform._playerTrigger.IsPlayerInside)
                {
                    _platform._waitUntilPlayerLeaves = true;
                }
            }
        }
        #endregion

        [SerializeField] private Transform _lockPoint;
        [SerializeField] private PlatformSwitchableRenderer _renderer;
        [SerializeField] private PlayerTrigger _playerTrigger;

        protected bool _waitUntilPlayerLeaves = false;
        protected int _lockedId = -1;
        protected PlatformStateClass _currentState;
        private PlayerController _player;
        protected PlatformState CurrentStateName => _currentState?.StateName ?? PlatformState.Disabled;

        private void Start()
        {
            _playerTrigger.OnPlayerEnterEvent += OnPlayerEnter;
            _playerTrigger.OnPlayerExitEvent += OnPlayerExit;
            _currentState = new ReadyState(this);
            _currentState.OnStateStart();
        }
        private void Update()
        {
            _currentState.Update();
        }

        protected void ChangeState(PlatformState state)
        {
            _currentState.OnStateEnd();
            switch (state)
            {
                case PlatformState.Moving: _currentState = new MovingState(this);break;
                case PlatformState.Disabled: _currentState = new DisabledState(this); break;
                case PlatformState.Blocked: _currentState = new BlockedState(this);break;
                default: _currentState = new ReadyState(this); break;
            }
            _renderer.SetState(_currentState.VisualState);
            _playerTrigger.SetActivity(_currentState.InnerTriggerActive);
            _currentState.OnStateStart();            
        }

        protected void OnPlayerEnter(PlayerController player)
        {
            _player = player;            
            _currentState.OnPlayerEnter();
        }
        protected void OnPlayerExit()
        {
            _waitUntilPlayerLeaves = false;
            _currentState.OnPlayerExit();            
        }

        abstract protected bool TryReachDestination();
        private void SetRendererState(PlatformState state) => _renderer?.SetState(state);
        private bool TryLockPlayer()
        {
            if (_player != null) return _player.TryLock(_lockPoint, out _lockedId);
            else return true;
        }
        private void UnlockPlayer()
        {
            if (_player != null) _player.Unlock(_lockedId);
        }
        private bool CanStartMovement()
        {
            if (_waitUntilPlayerLeaves) return false;
            else
            {
                return IsPlayerInSuitablePosition() && TryLockPlayer();
            }
        }

        private bool IsPlayerInSuitablePosition() => _player != null && _playerTrigger.IsPlayerFullyInside();

        virtual protected void OnDrawGizmosSelected()
        {
            if (_player != null) Gizmos.DrawSphere(_player.Position,0.5f);
        }
    }
}
