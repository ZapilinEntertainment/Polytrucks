using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks.AccountData {
	public sealed class GamePreferences : IGamePreferences
	{
        public LocalizationLanguage SelectedLanguage { get; private set; }

        public GamePreferences() {
            if (SelectedLanguage == LocalizationLanguage.Undefined)
            {
                if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
                {
                    SelectedLanguage = LocalizationLanguage.Russian;
                }
                else
                {
                    SelectedLanguage = LocalizationLanguage.English;
                }
            }
        }

        public void ChangeLanguage(LocalizationLanguage language) => SelectedLanguage = language;
    }
}
