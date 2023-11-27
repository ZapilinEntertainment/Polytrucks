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

            public PlatformStateClass(TransportingPlatform platform)
            {
                _platform = platform;
            }

            public virtual void OnStateStart() { }
            public virtual void Update() { }
            public virtual void OnStateEnd() { }
            public virtual void OnPlayerEnter(PlayerController player) { }
            public virtual void OnPlayerExit() { }
        }
        protected class ReadyState : PlatformStateClass
        {
            public ReadyState(TransportingPlatform platform) : base(platform) { }
            public override void OnStateStart()
            {
                _platform._playerTrigger.SetActivity(true);
                _platform.SetRendererState(PlatformState.Ready);
            }
            public override void OnPlayerEnter(PlayerController player)
            {
                if (player.TryLock(_platform._lockPoint)) {
                    _platform.ChangeState(PlatformState.Moving);
                }
                else
                {
                    _platform.ChangeState(PlatformState.Blocked);
                }
            }
        }
        protected class BlockedState : PlatformStateClass
        {
            public BlockedState(TransportingPlatform platform) : base(platform) { }

            public override void OnStateStart()
            {
                _platform.SetRendererState(PlatformState.Blocked);
                _platform._playerTrigger.SetActivity(true);
            }
            public override void OnPlayerExit()
            {
                _platform.ChangeState(PlatformState.Ready);
            }
        }
        protected class DisabledState : PlatformStateClass
        {
            public DisabledState(TransportingPlatform platform) : base(platform) { }
            public override void OnStateStart()
            {
                _platform.SetRendererState(PlatformState.Disabled);
                _platform._playerTrigger.SetActivity(false);
            }
        }
        protected class MovingState : PlatformStateClass
        {
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
                    _platform.ChangeState(PlatformState.Blocked);
                }
            }
        }
        #endregion

        [SerializeField] private Transform _lockPoint;
        [SerializeField] private PlatformSwitchableRenderer _renderer;
        [SerializeField] private PlayerTrigger _playerTrigger;

        private PlatformStateClass _currentState;

        private void Start()
        {
            _playerTrigger.OnPlayerEnterEvent += OnPlayerEnter;
            _currentState = new ReadyState(this);
            _currentState.OnStateStart();
        }
        private void Update()
        {
            _currentState.Update();
        }

        private void ChangeState(PlatformState state)
        {
            _currentState.OnStateEnd();
            switch (state)
            {
                case PlatformState.Moving: _currentState = new MovingState(this);break;
                case PlatformState.Disabled: _currentState = new DisabledState(this); break;
                case PlatformState.Blocked: _currentState = new BlockedState(this);break;
                default: _currentState = new ReadyState(this); break;
            }
            _currentState.OnStateStart();
        }

        protected void OnPlayerEnter(PlayerController player)
        {
            _currentState.OnPlayerEnter(player);
        }
        protected void OnPlayerExit()
        {
            _currentState.OnPlayerExit();
        }

        abstract protected bool TryReachDestination();
        private void SetRendererState(PlatformState state) => _renderer?.SetState(state);
    }
}
