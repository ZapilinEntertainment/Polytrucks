using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public enum QuestType : byte
	{
		Delivery, Supply, TimedDelivery
	}
	public abstract class QuestBase
	{
		public bool IsCompleted { get; protected set; } = false;
		public bool IsActive { get; protected set; } = false;
		abstract public bool UseMarkerTracking { get; }		
		public QuestType QuestType { get; protected set; }
		public Action OnProgressionChangedEvent, OnQuestStoppedEvent;		


		virtual public void StartQuest() => IsActive = true;
		public virtual void StopQuest()
		{
			IsActive = false;
			OnQuestStoppedEvent?.Invoke();
		}
		abstract public bool TryComplete();
        abstract public Vector3 GetTargetPosition();
        public abstract IQuestMessage FormNameMsg();
        public abstract IQuestMessage FormProgressionMsg();
    }
}
