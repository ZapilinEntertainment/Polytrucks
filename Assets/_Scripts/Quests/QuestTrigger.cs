using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {

    public class QuestTrigger : MonoBehaviour
	{
        [SerializeField] protected bool _canBeRestarted = false;
		[SerializeField] protected QuestPreset _preset;
        [SerializeField] protected PlayerTrigger _playerTrigger;
        protected QuestsManager _questManager;
        protected UIManager _uiManager;
        protected Localization _localization;
        protected QuestBase _trackingQuest;

        [Inject]
        public void Inject(QuestsManager manager, UIManager uiManager, Localization localization)
        {
            _questManager = manager;
            _uiManager = uiManager;
            _localization = localization;
        }

        private void Start()
        {
            _playerTrigger.OnPlayerEnterEvent += OnPlayerEnter;
        }

        protected void OnPlayerEnter(PlayerController player)
        {
            if (_trackingQuest != null) return;
            if (_questManager.TryStartQuest(_preset, out var quest, out var msgLabel))
            {
                _trackingQuest = quest;
                HideTrigger();
            }
            else
            {
                _uiManager.ShowAppearLabel(_playerTrigger.transform.position, _localization.GetLocalizedString(msgLabel));
            }
        }

        public void HideTrigger()
        {
            if (_canBeRestarted)
            {
                _playerTrigger.SetActivity(false);
                _trackingQuest.OnQuestCompletedEvent += Restart;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Restart()
        {
            _playerTrigger.SetActivity(true);
        }
    }
}
