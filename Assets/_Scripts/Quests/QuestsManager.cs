using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {

   
    public sealed class QuestsManager
	{
        private PlayerController _player;
        private Dictionary<QuestType, QuestBase> _activeQuests = new Dictionary<QuestType, QuestBase>();
        public Action<QuestBase> OnQuestStartedEvent;
        public IReadOnlyCollection<QuestBase> GetActiveQuests() => _activeQuests.Values;

        [Inject]
        public void Inject(PlayerController player)
        {
            _player = player;
        }
		public bool TryStartQuest(QuestPreset preset, out QuestBase quest, out LocalizedString message)
        {
            if (_activeQuests.ContainsKey(preset.QuestType))
            {
                message = LocalizedString.Refuse_AlreadyHaveSuchQuest;
                quest = null;
                return false;
            }
            else
            {
                if (preset.TryStartQuest(_player, out quest))
                {
                    StartTrackQuest(quest);
                    message = LocalizedString.QuestStarted;
                    return true;
                }
                else
                {
                    quest = null;
                    message = LocalizedString.CannotLoadCargo;
                    return false;
                }
            }
        }

       

        private void StartTrackQuest(QuestBase quest)
        {
            _activeQuests.Add(quest.QuestType, quest);
            OnQuestStartedEvent?.Invoke(quest);
        }
	}
}
