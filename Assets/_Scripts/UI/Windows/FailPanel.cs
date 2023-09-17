using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class FailPanel : EndGameWindow
	{
		//protected override int GetReward() => GameSettings.Current?.FailReward ?? 50;
        public void BUTTON_Restart()
		{
			SessionObjectsContainer.GameManager.RestartLevel();
		}
		public void BUTTON_BackToMenu()
		{
			SessionObjectsContainer.GameManager.ReturnToMenu();
		}
    }
}
