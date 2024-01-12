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
			[Tooltip("Trigger invokes signal to pass to new stage")] public CollectionActivatedTrigger Trigger;
            [Tooltip("Stage object will be activated when the stage starts")] public GameObject StageObject;
            [Tooltip("Activable script will be called when the stage starts")] public MonoBehaviour ActivableScript;

			public void Hide() => StageObject.SetActive(false);
		}

		[SerializeField] private RequireZoneStage[] _stages;
		private int _currentStage = 0;

        private void Start()
        {
			SubscribeToStage(_currentStage);
        }

		private void SubscribeToStage(int index)
		{
			//Debug.Log(_stages.Length);
			var stageTrigger = _stages[index].Trigger;
			if (stageTrigger != null) stageTrigger.OnConditionCompletedEvent += OnStageConditionComplete;
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
			if (_currentStage < _stages.Length)
			{
				var stage = _stages[_currentStage];

				var obj = stage.StageObject;
				if (obj != null) obj.SetActive(true);
				var script = stage.ActivableScript;
				if (script != null && script is IActivableMechanism) (script as IActivableMechanism).Activate();

				SubscribeToStage(_currentStage);
			}
		}
    }
}
