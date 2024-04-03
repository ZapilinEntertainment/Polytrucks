using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class QuestsPanel : MonoBehaviour
	{
        [SerializeField] private QuestTypeDefinedValues<QuestTrackerUI> _trackers;
        private QuestsManager _questsManager;

        [Inject]
        public void Inject(QuestsManager questsManager)
        {
            _questsManager = questsManager;
        }

        private void Start()
        {
            foreach (var tracker in _trackers) { tracker?.DisableTracker(); }

            _questsManager.OnQuestStartedEvent += OnQuestStarted;
            var activeQuests = _questsManager.GetActiveQuests();
            if (activeQuests.Count > 0)
            {
                foreach (var activeQuest in activeQuests) OnQuestStarted(activeQuest);
            }
        }

        private void OnQuestStarted(QuestBase quest)
        {
            _trackers[quest.QuestType].StartTracking(quest);
        }
    }
}
