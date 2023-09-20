using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class UIManager : SessionObject
	{
		[SerializeField] private VictoryWindow _debriefWindow;
		[SerializeField] private FailPanel _failPanel;
		[SerializeField] private GameObject _playerUI;

		private void i_ShowDebriefWindow()
		{
			_debriefWindow?.Show();
		}
		private void i_ShowFailPanel()
		{
            _failPanel.Show();
		}

		public static void ShowDebriefWindow() => SessionObjectsContainer.UIManager?.i_ShowDebriefWindow();
		public static void ShowFailPanel() => SessionObjectsContainer.UIManager?.i_ShowFailPanel();

        public override void OnSessionStart()
        {
            base.OnSessionStart();
			_playerUI.SetActive(true);
        }
        public override void OnSessionEnd()
        {
            base.OnSessionEnd();
			_playerUI.SetActive(false);
        }
    }
}
