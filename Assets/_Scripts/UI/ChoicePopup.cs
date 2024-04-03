using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public sealed class ChoicePopup : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] private GameObject _popupWindow;
		[SerializeField] private TMPro.TMP_Text _mainLabel, _leftLabel, _rightLabel;
		[SerializeField] private UnityEngine.UI.Image _leftButton, _rightButton;

		private bool _isActive = false;
		private LocalizedString _mainLabelString, _leftChoiceString, _rightChoiceString;
		private Localization _localization;
		private Action _leftChoiceCallback, _rightChoiceCallBack;

		[Inject]
		public void Inject(Localization localization, UIManager uiManager)
		{
			_localization = localization;
			transform.SetParent( uiManager.PopupHost);
		}
        private void Awake()
        {
			_popupWindow.SetActive(false);
        }

        private void Start()
        {
            _localization.Subscribe(this);
        }

		public void OnLocaleChanged()
		{
			if (_isActive) FulfillStrings();
		}

        public void ShowChoice(LocalizedString choiceString, LocalizedString leftChoice, LocalizedString rightChoice, Action leftChoiceCallback, Action rightChoiceCallback)
		{
			_mainLabelString = choiceString;
			_leftChoiceString = leftChoice;
			_rightChoiceString = rightChoice;

			_leftChoiceCallback = leftChoiceCallback;
			_rightChoiceCallBack = rightChoiceCallback;

			_isActive = true;
			FulfillStrings();
			_popupWindow.SetActive(true);
		}
		private void FulfillStrings()
		{
			_mainLabel.text = _localization.GetLocalizedString(_mainLabelString);
			_leftLabel.text = _localization.GetLocalizedString(_leftChoiceString);
			_rightLabel.text = _localization.GetLocalizedString(_rightChoiceString);
		}

		public void BUTTON_LeftChoice()
		{			
			_leftChoiceCallback?.Invoke();
            ClosePopup();
        }
		public void BUTTON_RightChoice()
		{			
			_rightChoiceCallBack?.Invoke();
            ClosePopup();
        }
		private void ClosePopup()
		{
			_isActive = false;
			_leftChoiceCallback = null;
			_rightChoiceCallBack = null;
			_popupWindow.SetActive(false);
		}
	}
}
