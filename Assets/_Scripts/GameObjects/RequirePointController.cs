using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public class RequirePointController : MonoBehaviour
	{
		[Serializable]
		public struct RequireZoneStage
		{
			public CollectionActivatedTrigger Trigger;
			public GameObject StageObject;

			public void Hide() => StageObject.SetActive(false);
		}

		[SerializeField] private RequireZoneStage[] _stages;
		private int _currentStage = 0;

        private void Awake()
        {
			SubscribeToStage(_currentStage);
        }

		private void SubscribeToStage(int index)
		{
			var stage = _stages[index];
			stage.Trigger.OnConditionCompletedEvent += OnStageConditionComplete;
		}
		private void UnsubscribeFromStage(int index)
		{
			var stageTrigger = _stages[index].Trigger;	
			if (stageTrigger != null) stageTrigger.OnConditionCompletedEvent-= OnStageConditionComplete;
		}

		private void OnStageConditionComplete()
		{
			_stages[_currentStage].Hide();
			UnsubscribeFromStage(_currentStage);
			_currentStage++;
			if (_currentStage < _stages.Length) SubscribeToStage(_currentStage);
		}
    }
}
