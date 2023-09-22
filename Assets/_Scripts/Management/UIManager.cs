using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class UIManager : SessionObject
	{
		[SerializeField] private VictoryWindow _debriefWindow;
		[SerializeField] private FailPanel _failPanel;
		[SerializeField] private GameObject _playerUI;

		public void ShowDebriefWindow()
		{
			_debriefWindow?.Show();
		}
		public void ShowFailPanel()
		{
            _failPanel.Show();
		}

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
