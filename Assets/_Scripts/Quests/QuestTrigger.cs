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
        [SerializeField] private ColourableRenderer[] _markerRenderers;
        protected QuestsManager _questManager;
        protected UIManager _uiManager;
        protected Localization _localization;
        protected QuestBase _trackingQuest;
        protected UIColorsPack _colorsPack;

        [Inject]
        public void Inject(QuestsManager manager, UIManager uiManager, Localization localization, UIColorsPack colorsPack)
        {
            _questManager = manager;
            _uiManager = uiManager;
            _localization = localization;
            _colorsPack= colorsPack;
        }

        private void Start()
        {
            _playerTrigger.OnPlayerEnterEvent += OnPlayerEnter;
            if (_markerRenderers != null)
            {
                var color = _colorsPack.GetQuestMarkerColor(_preset.QuestType);
                foreach (var renderer in _markerRenderers) renderer.SetColour(color);
            }
        }

        protected void OnPlayerEnter(PlayerController player)
        {
            if (_trackingQuest != null) return;
            if (_questManager.TryStartQuest(_preset, out var quest, out var msgLabel))
            {
                _trackingQuest = quest;
                HideTrigger();
            }
            _uiManager.ShowAppearLabel(_playerTrigger.transform.position, _localization.GetLocalizedString(msgLabel));
        }

        public void HideTrigger()
        {
            if (_canBeRestarted)
            {
                _playerTrigger.SetActivity(false);
                _trackingQuest.OnQuestFailedEvent += Restart;
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
