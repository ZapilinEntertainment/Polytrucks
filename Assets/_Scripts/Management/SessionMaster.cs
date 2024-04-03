using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks
{
    public sealed class SessionMaster : MonoBehaviour
    {
        [SerializeField] private bool _gameStartsOnFirstClick = true;
        private enum GameState : byte { AwaitForStart, Game, GameFinished, LoadingNextScene}
        private GameState _gameState = GameState.AwaitForStart;
        private SignalBus _signalBus;
        public bool SessionStarted { get; private set; }
        public bool IsPaused => false;

        [Inject]
        public void Inject(SignalBus signalBus)
        {
            _signalBus= signalBus;
        }

        private void Awake()
        {
            Input.multiTouchEnabled = false;
            Application.targetFrameRate = 60;
            //Time.fixedDeltaTime = 0.0025f;
        }

        private IEnumerator Start()
        {
            if (_gameStartsOnFirstClick) yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButtonDown(0) );
            StartLevel();
        }

#if UNITY_EDITOR
        private void Update()
        {
           // if (Input.GetKeyDown("f")) OnLevelFailed();
           // if (Input.GetKeyDown("w")) OnLevelCompleted();
        }
#endif

        private void StartLevel()
        {
            SessionStarted = true;
            _gameState = GameState.Game;
            _signalBus.Fire<SessionStartSignal>();
           // AnalyticsManager.OnLevelStarted();
        }
        public void OnLevelCompleted()
        {
            if (_gameState == GameState.Game)
            {
                _gameState = GameState.GameFinished;
                _signalBus.Fire<SessionStopSignal>();
                
                //Saves.AddMoney(GameSettings.Current.VictoryReward);
                //UIManager.ShowDebriefWindow();
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
                _signalBus.Fire<SessionStopSignal>();
                //Saves.AddMoney(GameSettings.Current.FailReward);
                //UIManager.ShowFailPanel();
               // AnalyticsManager.OnLevelFailed();
            }
        }

    }
}
