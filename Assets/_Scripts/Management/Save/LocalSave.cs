using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace ZE.Polytrucks {
    public sealed class LocalSave : ISaveContainer
    {
        private const string PLAYER_POSITION_KEY = "PlayerPosition", LOCALE_KEY = "LocaleLanguage";       

        public void SavePlayerPosition(VirtualPoint point)
        {
            var data = point.Encode();
            PlayerPrefs.SetString(PLAYER_POSITION_KEY, data);
        }
        public VirtualPoint LoadPlayerPosition()
        {
            string data = PlayerPrefs.GetString(PLAYER_POSITION_KEY, null);
            if (data != null && data.Length > 0)
            {
                return VirtualPoint.Decode(data);
            }
            else return new VirtualPoint();
        }

        public LocalizationLanguage LoadLocale() => (LocalizationLanguage)PlayerPrefs.GetInt(LOCALE_KEY);
        public void SaveLocale(LocalizationLanguage locale) => PlayerPrefs.SetInt(LOCALE_KEY, (int)locale);
    }
}
