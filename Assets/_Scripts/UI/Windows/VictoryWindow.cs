using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class VictoryWindow : EndGameWindow
	{
		[SerializeField] private TMPro.TMP_Text _killCountLabel, _headshotsLabel, _timeLabel;
        //protected override int GetReward() => GameSettings.Current?.VictoryReward ?? 200;

        protected override void i_Show()
        {
            UpdateContent();
            base.i_Show();
        }
        private void UpdateContent()
        {
           
        }
        public void BUTTON_Continue()
		{
			SessionObjectsContainer.GameManager.OnVictoryPanelClosed();
		}
	}
}
