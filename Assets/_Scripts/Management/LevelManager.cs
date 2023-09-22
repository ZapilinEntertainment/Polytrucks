using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;

namespace ZE.Polytrucks
{
    [System.Serializable] public enum LevelFinishMode : byte { NoActions, Repeat, LoadNext }
    public sealed class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelFinishMode _levelFinishMode = LevelFinishMode.NoActions;
        public bool IsLevelLoaded { get; private set; }
        public LevelSettings CurrentLevel { get; private set; }
        public Action<LevelSettings> OnLevelLoadedEvent;
        public Action OnLevelClearEvent;
        public static int LaunchedLevelIndex { get; private set; }

        private void Awake()
        {
            CurrentLevel = FindObjectOfType<LevelSettings>();
            IsLevelLoaded = CurrentLevel != null;
            if (IsLevelLoaded) HandleLevel(CurrentLevel);
           
        }
        public void Subscribe(ILevelSubscriber ils)
        {
            OnLevelLoadedEvent += ils.OnLevelLoaded;
            OnLevelClearEvent += ils.OnLevelClear;
            if (IsLevelLoaded) ils.OnLevelLoaded(CurrentLevel);
        }
        
        private void HandleLevel(LevelSettings settings)
        {
            // actions
            OnLevelLoadedEvent?.Invoke(settings);
        }

    }
}
