using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public abstract class SessionObject : MonoBehaviour, ISessionObject, IDisposable
	{
        protected bool GameSessionActive => _sessionStarted;
		private bool _sessionStarted = false, _isPaused = false, _isExiting = false;
        protected SignalBus _signalBus;

        [Inject]
        virtual public void Setup(SignalBus signalBus, SessionMaster sessionMaster)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SessionStartSignal>(OnSessionStart);
            _signalBus.Subscribe<SessionStopSignal>(OnSessionEnd);
            _signalBus.Subscribe<SessionPauseSignal>(OnSessionPause);
            _signalBus.Subscribe<SessionResumeSignal>(OnSessionResume);

            _sessionStarted = sessionMaster.SessionStarted;
            _isPaused = sessionMaster.IsPaused;
        }

        virtual public void OnSessionEnd()
        {
            _sessionStarted = false;
        }

        virtual public void OnSessionStart()
        {
            _sessionStarted = true;
        }
        virtual public void OnSessionPause()
        {
            _isPaused = true;
        }
        virtual public void OnSessionResume()
        {
            _isPaused = false;
        }

        public void Dispose()
        {
            if (_signalBus != null && !_isExiting)
            {
                _signalBus.Unsubscribe<SessionStartSignal>(OnSessionStart);
                _signalBus.Unsubscribe<SessionStopSignal>(OnSessionEnd);
                _signalBus.Unsubscribe<SessionPauseSignal>(OnSessionPause);
                _signalBus.Unsubscribe<SessionResumeSignal>(OnSessionResume);
            }
        }
        private void OnApplicationQuit()
        {
            _isExiting = true;
        }
    }
}
