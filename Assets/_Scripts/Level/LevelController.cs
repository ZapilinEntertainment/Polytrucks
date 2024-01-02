using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class LevelController : MonoBehaviour
	{
		[SerializeField] protected RecoveryPoint _defaultRecoveryPoint;
		[SerializeField] protected QuestPreset _startQuestPreset;
		protected RecoverySystem _recoverySystem;
		protected QuestsManager _questsManager;

		[Inject]
		public void Inject(RecoverySystem recoverySystem, QuestsManager questsManager)
		{
			_recoverySystem = recoverySystem;			
			_questsManager = questsManager;
		}

        private void Start()
        {
            _recoverySystem.SetRecoveryPoint(_defaultRecoveryPoint);
			if (!_questsManager.TryStartQuest(_startQuestPreset, out var quest, out var message)) Debug.LogError(message.ToString());
        }
    }
}
