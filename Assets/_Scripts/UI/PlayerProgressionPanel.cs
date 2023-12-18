using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class PlayerProgressionPanel : MonoBehaviour
	{
		[SerializeField] private ProgressionBar _progressBar;
		[SerializeField] private TMPro.TMP_Text _levelLabel;
		private PlayerData _playerData;

		[Inject]
		public void Inject(PlayerData playerData, SignalBus signalBus)
		{
			_playerData= playerData;
			signalBus.Subscribe<PlayerLevelUpSignal>(OnPlayerLevelUp);			
		}

        private void Start()
        {
			var exp = _playerData.Experience;
			_progressBar.Setup(exp.PointsToNextLevel, exp.ProgressPercent);
			_levelLabel.text = exp.Level.ToString();
			exp.OnExperienceCountChangedEvent += OnExperiencePercentChanged;
        }
		private void OnPlayerLevelUp(PlayerLevelUpSignal signal)
		{
			int level = signal.Level;
			_levelLabel.text = level.ToString();
			// make an effect for the level
		}
		private void OnExperiencePercentChanged()
		{
			float percent = _playerData.Experience.ProgressPercent;
			_progressBar.SetProgress(percent);
		}
    }
}
