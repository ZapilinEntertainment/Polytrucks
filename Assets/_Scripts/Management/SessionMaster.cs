using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks
{
    public sealed class SessionMaster : MonoBehaviour
    {
        [SerializeField] private bool _gameStartsOnFirstClick = true;
        private enum GameState : byte { AwaitForStart, Game, GameFinished, LoadingNextScene}
        private GameState _gameState = GameState.AwaitForStart;
        public bool SessionStarted { get; private set; }
        public Action OnSessionStartEvent, OnSessionEndEvent, OnSessionPauseEvent, OnSessionResumeEvent;

        private void Awake()
        {
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = 60;
            //Time.fixedDeltaTime = 0.0025f;
        }

        public void SubscribeToSessionEvents(ISessionObject iso)
        {
            OnSessionStartEvent += iso.OnSessionStart;
            OnSessionEndEvent += iso.OnSessionEnd;
            OnSessionPauseEvent += iso.OnSessionPause;
            OnSessionResumeEvent += iso.OnSessionResume;
            if (SessionStarted) iso.OnSessionStart();
        }
        public void UnsubscribeFromSessionEvents(ISessionObject iso)
        {
            OnSessionStartEvent -= iso.OnSessionStart;
            OnSessionEndEvent -= iso.OnSessionEnd;
        }

        private IEnumerator Start()
        {
            if (_gameStartsOnFirstClick) yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0) );
            StartLevel();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown("f")) OnLevelFailed();
            if (Input.GetKeyDown("w")) OnLevelCompleted();
        }
#endif

        private void StartLevel()
        {
            SessionStarted = true;
            _gameState = GameState.Game;
            OnSessionStartEvent?.Invoke();
           // AnalyticsManager.OnLevelStarted();
        }
        public void OnLevelCompleted()
        {
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.GameFinished;
                OnSessionEndEvent?.Invoke();
                
                //Saves.AddMoney(GameSettings.Current.VictoryReward);
                UIManager.ShowDebriefWindow();
                //AnalyticsManager.OnLevelCompleted();
            }
        }
        public void OnVictoryPanelClosed()
        {
            if (_gameState == GameState.GameFinished)
            {
                _gameState = GameState.LoadingNextScene;
                //if (TEST_doNotReturnToHub) LevelManager.LoadNextLevel();
                //else LevelManager.LoadHubScene();
            }
        }

        public void OnLevelFailed()
        {
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.GameFinished;
                OnSessionEndEvent?.Invoke();
                //Saves.AddMoney(GameSettings.Current.FailReward);
                UIManager.ShowFailPanel();
               // AnalyticsManager.OnLevelFailed();
            }
        }
        public void RestartLevel()
        {
            if (_gameState == GameState.GameFinished)
            {
                _gameState = GameState.LoadingNextScene;
                LevelManager.RestartLevel();
            }
        }
        public void ReturnToMenu()
        {
            if (_gameState != GameState.LoadingNextScene)
            {
                _gameState = GameState.LoadingNextScene;
                LevelManager.LoadHubScene();
            }
        }
    }
}
