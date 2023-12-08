using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {	
	public abstract class QuestBase
	{
		public bool IsCompleted { get; protected set; } = false;
		public bool IsActive { get; protected set; } = false;
		public bool CanBeRejected { get; protected set; } = true;
		abstract public bool UseMarkerTracking { get; }		
		public QuestType QuestType { get; protected set; }
		public Action OnProgressionChangedEvent, OnQuestStoppedEvent, OnQuestCompletedEvent, OnQuestFailedEvent;		


		virtual public void StartQuest() => IsActive = true;
		public virtual void StopQuest()
		{
			IsActive = false;
			OnQuestStoppedEvent?.Invoke();
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
					return true;
				}
				else return false;
			}
			else return true;
		}		
		abstract public bool CheckConditions();
        abstract public Vector3 GetTargetPosition();
        public abstract IQuestMessage FormNameMsg();
        public abstract IQuestMessage FormProgressionMsg();
    }
}
