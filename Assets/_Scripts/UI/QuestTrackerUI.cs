using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace ZE.Polytrucks {
	public class QuestTrackerUI : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] private TMP_Text _questName, _questProgress;
		[SerializeField] private RectTransform _marker;
		[SerializeField] private GameObject _rejectButton;
		private bool _useMarkerTracking = false;
		private QuestBase _trackingQuest;
		private Localization _localization;
		private Camera _camera;
		private ChoicePopup _choicePopup;

		[Inject]
		public void Inject(Localization localization, CameraController cameraController, ChoicePopup choicePopup)
		{
			_localization = localization;
			_localization.OnLocaleChangedEvent += OnLocaleChanged;
			_camera = cameraController.Camera;
			_choicePopup = choicePopup;
		}

		public void StartTracking(QuestBase quest)
		{
			if (_trackingQuest != null) StopTracking();
			
			_trackingQuest.OnProgressionChangedEvent += OnProgressionChanged;			
			_marker.gameObject.SetActive(true);
			UpdateTextDescriptions();
			_useMarkerTracking = _trackingQuest.UseMarkerTracking;
			_rejectButton.SetActive(quest.CanBeRejected);
		}
		private void OnProgressionChanged()
		{
            _questProgress.text = _trackingQuest.FormProgressionMsg().ToString(_localization);
        }
		public void StopTracking()
		{
			_trackingQuest.OnProgressionChangedEvent -= OnProgressionChanged;
		}
		private void UpdateTextDescriptions()
		{
            _questName.text = _trackingQuest.FormNameMsg().ToString(_localization);
            OnProgressionChanged();
        }

        private void Update()
        {
            if (_useMarkerTracking)
			{
				Vector3 scrpos = _camera.WorldToScreenPoint(_trackingQuest.GetTargetPosition());
				scrpos.x = Mathf.Clamp(scrpos.x, 0f, Screen.width - _marker.rect.width);
				scrpos.y = Mathf.Clamp(scrpos.y, 0f, Screen.height - _marker.rect.height);
				_marker.position = scrpos;
			}
        }

        public void OnLocaleChanged(LocalizationLanguage language) => UpdateTextDescriptions();

		public void BUTTON_RejectQuest()
		{
			if (_trackingQuest != null)
			{
				_choicePopup.ShowChoice(LocalizedString.Ask_StopQuest, LocalizedString.StopQuest, LocalizedString.Cancel, StopTrackingQuest, null);
			}
		}
		private void StopTrackingQuest()
		{

		}
    }
}
