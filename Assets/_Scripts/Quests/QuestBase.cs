using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {	
	public abstract class QuestBase : IWorldPositionable
	{
		public bool IsCompleted { get; protected set; } = false;
		public bool IsActive { get; protected set; } = false;
		public bool CanBeRejected { get; protected set; } = true;
		abstract public bool UseMarkerTracking { get; }
		abstract public QuestType QuestType { get; }

		public Action OnProgressionChangedEvent, OnQuestStoppedEvent, OnQuestCompletedEvent, OnQuestFailedEvent;		

		virtual public void StartQuest() => IsActive = true;
		
		public void RejectQuest()
		{
			OnQuestFailedEvent?.Invoke();
			StopQuest();
		}
		public bool TryCompleteQuest()
		{
			if (!IsCompleted)
			{
				if (!IsActive) return false;
				if (CheckConditions())
				{
					IsCompleted = true;
					OnQuestCompletedEvent?.Invoke();
					StopQuest();
					return true;
				}
				else return false;
			}
			else return true;
		}
        protected virtual void StopQuest()
        {
            IsActive = false;
            OnQuestStoppedEvent?.Invoke();
        }
        abstract public bool CheckConditions();

		virtual public int GetExperienceReward(GameSettings gameSettings) => gameSettings.GetQuestExperienceReward(QuestType);

        abstract public Vector3 GetWorldPosition();
        public abstract IQuestMessage FormNameMsg();
        public abstract IQuestMessage FormProgressionMsg();
    }
}
