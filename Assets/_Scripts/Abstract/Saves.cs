using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks
{
    public static class Saves
    {
        public static Action<int> OnMoneyCountChangedEvent;
        private static bool TEST_everythingIsUnlocked = false;
        private const string MONEY_KEY = "Money", COMPLETE_MISSIONS_KEY = "CompleteMissionsCount", LEVEL_KEY = "LevelIndex", FIRST_ADVICE_KEY = "FirstAdvice", BIOME_KEY = "Biome", 
            BIOME_UNLOCKED_MASK_KEY = "BiomesUnlockMask", SOUND_VOLUME_KEY = "SoundVolume", MUSIC_VOLUME_KEY = "MusicVolume";

        #region money
        public static void AddMoney(int x)
        {
            if (x > 0) SetMoney(GetMoney() + x);
        }
        public static bool TrySpendMoney(int x)
        {
            int money = GetMoney();
            if (money >= x)
            {
                SetMoney(money - x);
                return true;
            }
            else return false;
        }
        public static int GetMoney() => PlayerPrefs.GetInt(MONEY_KEY, 0);
        private static void SetMoney(int x)  {
            PlayerPrefs.SetInt(MONEY_KEY, x);
            OnMoneyCountChangedEvent?.Invoke(x);
        }
        #endregion

        #region progression

        public static int GetCompleteMissionsCount() => PlayerPrefs.GetInt(COMPLETE_MISSIONS_KEY, 0);
        public static int GetLevelIndex() => PlayerPrefs.GetInt(LEVEL_KEY, 1);
        public static void TEST_SetProgressionPoint(Biome biome, int index)
        {
            SetBiome(biome);
            if (!biome.IsLast())
            {
                for (var i = BiomeExtensions.GetLast(); i > biome; i--)
                {
                    ClearBiomeLevelsMask(i);
                }
            }
            int biomesUnlockMask = 0;
            for (int i = 0; i <= (int)biome; i++)
            {
                biomesUnlockMask |= (1 << i);

                int levelsMask = 1;
                Biome biomeId = (Biome)i;
                for (int j = 1; j <= biomeId.GetTotalLevelsCount(); j++)
                {
                    levelsMask |= (1 << j);
                }
                SetBiomeLevelMaskKey(biomeId, levelsMask);
            }
            SetBiomesUnlockMask(biomesUnlockMask);

            SetLevelIndex(index);
            int currentBiomeLevelsMask = 0;
            if (index > biome.GetTotalLevelsCount()) index = biome.GetTotalLevelsCount();
            for (int j = 0; j <= index; j++)
            {
                currentBiomeLevelsMask |= (1 << j);
            }
            //Debug.Log(currentBiomeLevelsMask);
            SetBiomeLevelMaskKey(biome, currentBiomeLevelsMask);
        }
        
        public static int IncreaseLevel()
        {
            var currentBiome = GetCurrentBiome();
            int levelIndex = GetLevelIndex();
            UnlockLevel(currentBiome, levelIndex);

            levelIndex++;
            if (levelIndex <= currentBiome.GetTotalLevelsCount())
            {
                SetLevelIndex(levelIndex);
                UnlockLevel(currentBiome, levelIndex);
            }            
            else
            {                
                IncreaseCurrentBiome();
                SetLevelIndex(1);
            }
            PlayerPrefs.SetInt(COMPLETE_MISSIONS_KEY, GetCompleteMissionsCount() + 1);
            PlayerPrefs.Save();
            return levelIndex;
        }
        public static bool IsFirstStart()
        {
            const string KEY = "FirstStart";
            if (PlayerPrefs.GetInt(KEY, 0) == 0)
            {
                PlayerPrefs.SetInt(KEY, 1);
                return true;
            }
            else return false;
        }
        public static bool IsFirstAdviceShown() => PlayerPrefs.GetInt(FIRST_ADVICE_KEY, 0) != 0;
        public static void CountFirstAdvice() => PlayerPrefs.SetInt(FIRST_ADVICE_KEY, 1);

        public static Biome GetCurrentBiome() => (Biome)PlayerPrefs.GetInt(BIOME_KEY, 0);
        public static void IncreaseCurrentBiome()
        {
            var nextBiome = GetCurrentBiome().GetNext();
            SetBiome(nextBiome);
            UnlockLevel(nextBiome, 1);
            UnlockBiome(nextBiome);
        }
        public static bool IsBiomeUnlocked(Biome biome) {
            if (TEST_everythingIsUnlocked) return true;
            if (biome == 0) return true;
            else {
                int mask = LoadBiomesMask(),
                    biomeMaskValue = 1 << ((int)biome);
                return (mask & biomeMaskValue) != 0;
            }
        }
        private static int LoadBiomesMask() => PlayerPrefs.GetInt(BIOME_UNLOCKED_MASK_KEY, 1);
        public static void UnlockBiome(Biome biome)
        {
            int mask = LoadBiomesMask(), biomeMaskValue = 1 << ((int)biome);
            if ((mask & biomeMaskValue) == 0)
            {
                mask |= biomeMaskValue;
                SetBiomesUnlockMask(mask);
            }
        }
        public static bool IsLevelUnlocked(Biome biome, int level)
        {
            if (TEST_everythingIsUnlocked) return true;
            if (IsBiomeUnlocked(biome))
            {
                if (level == 1) return true;
                else
                {
                    int mask = GetBiomeLevelsMask(biome),
                        levelVal = 1 << level;
                    //Debug.Log(mask);
                    return (mask & levelVal) != 0;
                }
            }
            else return false;
        }
        public static void UnlockLevel(Biome biome, int level)
        {
            int mask = GetBiomeLevelsMask(biome),
                levelVal = 1 << level;
            if ((mask & levelVal) == 0)
            {
                mask |= levelVal;
                PlayerPrefs.SetInt(GetBiomeLevelMaskKey(biome), mask);
            }
        }
        private static int GetBiomeLevelsMask(Biome biome) => PlayerPrefs.GetInt(GetBiomeLevelMaskKey(biome), 1);

        #region subfunctions
        private static string GetBiomeLevelMaskKey(Biome biome) => biome.ToString() + "LevelMask";
        private static void SetBiomeLevelMaskKey(Biome biome, int mask)
        {
            PlayerPrefs.SetInt(GetBiomeLevelMaskKey(biome), mask);
            OnProgressionChanged();
        }
        private static void ClearBiomeLevelsMask(Biome biome)
        {
            PlayerPrefs.DeleteKey(GetBiomeLevelMaskKey(biome));
            OnProgressionChanged();
        }

        private static void SetBiome(Biome biome)
        {
            PlayerPrefs.SetInt(BIOME_KEY, (int)biome);
            OnProgressionChanged();
        }
        private static void SetLevelIndex(int x)
        {
            PlayerPrefs.SetInt(LEVEL_KEY, x);
            OnProgressionChanged();
        }
        private static void SetBiomesUnlockMask(int mask) => PlayerPrefs.SetInt(BIOME_UNLOCKED_MASK_KEY, mask);
        private static void OnProgressionChanged()
        {
            
        }
        #endregion
        #endregion
        public static float LoadSoundEffectsVolume() => PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);
        public static void SetSoundEffectsVolume(float x) => PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, x);
        public static float LoadMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        public static void SetMusicEffectsVolume(float x) => PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, x);

        public static void SaveValues() => PlayerPrefs.Save();
        public static void TEST_UnlockAll() => TEST_everythingIsUnlocked = true;
    }
}
