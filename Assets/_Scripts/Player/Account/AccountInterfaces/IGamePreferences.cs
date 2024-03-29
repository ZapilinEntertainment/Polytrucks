using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IGamePreferences 
	{
		public LocalizationLanguage SelectedLanguage { get; }
		public void ChangeLanguage(LocalizationLanguage language);
    }
}
