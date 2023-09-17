using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class SessionObject : MonoBehaviour, ISessionObject
	{
        protected bool GameSessionActive => _sessionStarted;
		private bool _sessionStarted = false, _isPaused = false;

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

        private void Awake()
        {
            SessionObjectsContainer.GameManager.SubscribeToSessionEvents(this);
            OnAwake();
        }
        virtual protected void OnAwake() { }

        protected void Unsubscribe()
        {
            var gm = SessionObjectsContainer.GameManager;
            if (gm != null) gm.UnsubscribeFromSessionEvents(this);
        }

    }
}
