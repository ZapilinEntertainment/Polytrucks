using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

        private void i_LoadNextLevel()
        {
            switch (_levelFinishMode) {
                case LevelFinishMode.LoadNext: LoadLevel(Saves.GetLevelIndex()); break;
                case LevelFinishMode.Repeat: i_RestartLevel();break;
            }
        }
        private void i_RestartLevel()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        private void LoadLevel(int levelIndex)
        {
            s_LoadScene(levelIndex, Saves.GetCurrentBiome());
        }

        public static void LoadNextLevel()
        {
            var levelManager = SessionObjectsContainer.LevelManager;
            if (levelManager == null)
            {
                print("no level manager");
                return;
            }
            else levelManager.i_LoadNextLevel();
        }
        public static void RestartLevel()
        {
            var levelManager = SessionObjectsContainer.LevelManager;
            if (levelManager == null)
            {
                print("no level manager");
                return;
            }
            else levelManager.i_RestartLevel();
        }
        public static void s_LoadScene(int x, Biome biome)
        {
            // asset bundles
            string levelName = "Level_" + x.ToString()+'_' + biome.ToString();
            LaunchedLevelIndex = x;
            SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        }
        public static void LoadHubScene()
        {
            SceneManager.LoadSceneAsync("HubScene");
        }
    }
}
