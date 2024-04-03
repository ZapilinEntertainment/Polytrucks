using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class PlayerProgressionPanel : MonoBehaviour
	{
		[SerializeField] private ProgressionBar _progressBar;
		[SerializeField] private TMPro.TMP_Text _levelLabel;
		private IAccountDataAgent _accountDataAgent;
		private Experience _experience;

		[Inject]
		public void Inject(IAccountDataAgent accountDataAgent, SignalBus signalBus)
		{
			_accountDataAgent = accountDataAgent;
			signalBus.Subscribe<PlayerLevelUpSignal>(OnPlayerLevelUp);			
		}

        private void Start()
        {
            _experience = _accountDataAgent.PlayerDataAgent.Experience;
			_progressBar.Setup(Mathf.Clamp( _experience.PointsToNextLevel - 1,1,9), _experience.Points, _experience.PointsToNextLevel);
			_levelLabel.text = _experience.Level.ToString();
            _experience.OnExperienceCountChangedEvent += OnExperienceCountChanged;
        }
		private void OnPlayerLevelUp(PlayerLevelUpSignal signal)
		{
			int level = signal.Level;
			_levelLabel.text = level.ToString();
			// make an effect for the level
		}
		private void OnExperienceCountChanged()
		{
			_progressBar.SetProgress(_experience.Points, _experience.PointsToNextLevel);
		}
    }
}
