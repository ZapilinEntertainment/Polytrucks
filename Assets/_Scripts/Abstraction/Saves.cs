using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks
{
    public class Saves
    {
        public static Action<int> OnMoneyCountChangedEvent;
        private const string MONEY_KEY = "Money",  
             SOUND_VOLUME_KEY = "SoundVolume", MUSIC_VOLUME_KEY = "MusicVolume";

        #region money
        public void AddMoney(int x)
        {
            if (x > 0) SetMoney(GetMoney() + x);
        }
        public bool TrySpendMoney(int x)
        {
            int money = GetMoney();
            if (money >= x)
            {
                SetMoney(money - x);
                return true;
            }
            else return false;
        }
        public int GetMoney() => PlayerPrefs.GetInt(MONEY_KEY, 0);
        private void SetMoney(int x)  {
            PlayerPrefs.SetInt(MONEY_KEY, x);
            OnMoneyCountChangedEvent?.Invoke(x);
        }
        #endregion

        public bool IsFirstStart()
        {
            const string KEY = "FirstStart";
            if (PlayerPrefs.GetInt(KEY, 0) == 0)
            {
                PlayerPrefs.SetInt(KEY, 1);
                return true;
            }
            else return false;
        }

        public float LoadSoundEffectsVolume() => PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);
        public void SetSoundEffectsVolume(float x) => PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, x);
        public float LoadMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        public void SetMusicEffectsVolume(float x) => PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, x);

        public void SaveValues() => PlayerPrefs.Save();
    }
}
