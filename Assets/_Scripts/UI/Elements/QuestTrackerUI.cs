using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using DG.Tweening;
using System;

namespace ZE.Polytrucks {
	public class QuestTrackerUI : MonoBehaviour, IDynamicLocalizer
	{
		[Serializable]
		private class TrackerAnimator
		{
			[SerializeField] private float _completeMarkViewTime = 3f;
			[SerializeField] private CanvasGroup _questInfoGroup, _completeMarkGroup;
			private bool _animationInProgress = false;
            public bool IsAnimating => _animationInProgress;
            private Sequence _animationSequence;
			public Action OnAnimationEndsEvent;
			
			private const float DARKEN_TIME = 0.3f, COMPLETE_MARK_APPEAR_TIME = 0.5f;

			public void StartAnimation()
			{
				if (!_animationInProgress)
				{
					_animationInProgress = true;

                    _completeMarkGroup.gameObject.SetActive(true);
                    _animationSequence = DOTween.Sequence();
                    _animationSequence.Append(_questInfoGroup.DOFade(0.5f, DARKEN_TIME));
					_animationSequence.Append(_completeMarkGroup.DOFade(1f, COMPLETE_MARK_APPEAR_TIME));
					_animationSequence.AppendInterval(_completeMarkViewTime);
					_animationSequence.AppendCallback(OnHideEffectEnds);
                }
			}
			public void StopAnimation()
			{
				if (_animationInProgress)
				{
					_animationSequence.Kill();
					_animationInProgress = false;
				}
			}
			public void ReturnToStartValues()
			{
                _questInfoGroup.alpha = 1.0f;
                _completeMarkGroup.alpha = 0.0f;
                _completeMarkGroup.gameObject.SetActive(false);
            }
            private void OnHideEffectEnds()
            {
				_animationInProgress = false;
				_animationSequence = null;
				OnAnimationEndsEvent?.Invoke();
            }
        }

		
		[SerializeField] private TMP_Text _questName, _questProgress;
		[SerializeField] private GameObject _rejectButton;
		[SerializeField] private TrackerAnimator _animator;
		
		private bool _useMarkerTracking = false;
		private QuestBase _trackingQuest;
		private Localization _localization;
		private ObjectScreenMarker _marker;
		private UIManager _uiManager;
		private ChoicePopup _choicePopup;
		private UIColorsPack _colorsPack;

		[Inject]
		public void Inject(Localization localization,  UIManager uiManager, ChoicePopup choicePopup, UIColorsPack colorsPack)
		{
			_localization = localization;			
			_uiManager= uiManager;
			_choicePopup= choicePopup;
			_colorsPack= colorsPack;
		}
        private void Start()
        {
            _localization.Subscribe(this);
			_animator.OnAnimationEndsEvent += () => SetVisibility(false);
        }

        public void StartTracking(QuestBase quest)
		{
			if (_trackingQuest != null) StopTracking();

			if (_animator.IsAnimating)  _animator.StopAnimation();
			_animator.ReturnToStartValues();

            _trackingQuest = quest;
			_trackingQuest.OnProgressionChangedEvent += OnProgressionChanged;
			_trackingQuest.OnQuestStoppedEvent += StopTracking;
			
			UpdateTextDescriptions();
			_useMarkerTracking = _trackingQuest.UseMarkerTracking;
            if (_useMarkerTracking)
			{
				_marker = _uiManager.GetObjectMarker();
				_marker.StartTracking(_trackingQuest, _colorsPack.GetQuestMarkerColor(quest.QuestType));
			}


            _rejectButton.SetActive(quest.CanBeRejected);
			SetVisibility(true);
		}


        private void OnProgressionChanged()
		{
            _questProgress.text = _trackingQuest.FormProgressionMsg().ToString(_localization);
        }
		private void StopTracking()
		{
			if (_trackingQuest != null)
			{
				_trackingQuest.OnProgressionChangedEvent -= OnProgressionChanged;
				
				if (_useMarkerTracking) _marker.StopTracking();
                _rejectButton.SetActive(false);

				if (_trackingQuest.IsCompleted) _animator.StartAnimation(); else SetVisibility(false);

                _trackingQuest = null;
            }
		}
		

		private void UpdateTextDescriptions()
		{
            _questName.text = _trackingQuest.FormNameMsg().ToString(_localization);
            OnProgressionChanged();
        }

        public void OnLocaleChanged() => UpdateTextDescriptions();

		public void BUTTON_RejectQuest()
		{
			if (_trackingQuest != null)
			{
				_choicePopup.ShowChoice(LocalizedString.Ask_StopQuest, LocalizedString.StopQuest, LocalizedString.Cancel, RejectQuest, null);
			}
		}
		private void RejectQuest()
		{
			if (_trackingQuest.CanBeRejected) // additional excessive check
			{
				_trackingQuest.RejectQuest();
			}
		}
		private void SetVisibility(bool x) => gameObject.SetActive(x);

		public void DisableTracker()
		{
			if (_trackingQuest != null) StopTracking();
			SetVisibility(false);
		}
    }
}
