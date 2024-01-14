using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UnityEngine.UI;

namespace ZE.Polytrucks {
	public struct ActionContainer
	{
		public string CostLabel;
        public LocalizedString MainLabel, RejectionLabel;
		public Vector3 WorldPos;
		public Func<bool> ResultFunc;

		public bool ActionCheck()
		{
			if (ResultFunc != null) return ResultFunc();
			else return false;
		}
	} 
	public sealed class ActionPanel : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] private TMPro.TMP_Text _mainLabel, _costLabel, _rejectionLabel;
		[SerializeField] private Image _icon;
		[SerializeField] private GameObject _labelObject;
		[SerializeField] private float _rejectionLabelTime = 2f;
		private bool _isShowing = false;
		private int _actionID = 0;
		private LocalizedString _mainLabelString, _rejectionLabelString;
		private ActionContainer _currentActionContainer;
		private CameraController _cameraController;
		private Localization _localization;

		[Inject]
		public void Inject(CameraController cameraController, Localization localization)
		{
			_cameraController= cameraController;
			_localization= localization;			
		}

        private void Start()
        {
            _labelObject.SetActive(false);
			_rejectionLabel.enabled = false;
            _localization.Subscribe(this);
        }
        public int Show(ActionContainer actionContainer)
		{
			_actionID++;
			_currentActionContainer = actionContainer;
			_mainLabelString = actionContainer.MainLabel;
			_costLabel.text = actionContainer.CostLabel;
			_rejectionLabelString = actionContainer.RejectionLabel;
			LocalizeStrings();
			
			SetActivity(true);
            Update();
            return _actionID;
		}
		private void LocalizeStrings()
		{
			_mainLabel.text = _localization.GetLocalizedString(_mainLabelString);
			_rejectionLabel.text = _localization.GetLocalizedString(_rejectionLabelString);
		}
        private void Update()
        {
            if (_isShowing)
			{
                transform.position = _cameraController.Camera.WorldToScreenPoint(_currentActionContainer.WorldPos);
				if (_rejectionLabel.enabled)
				{
					var color = _rejectionLabel.color;
					color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / _rejectionLabelTime);
					_rejectionLabel.color = color;
					if (color.a == 0f) _rejectionLabel.enabled = false;
                }
            }
        }
        public void Hide(int id)
		{
			if (_actionID == id) i_Hide();
		}
		private void i_Hide()
		{
            _actionID = -1;
            SetActivity(false);
        }
		public void SetActivity(bool x)
		{
			_isShowing = x;
			_rejectionLabel.enabled = false;
			_labelObject.SetActive(x);
		}

		public void BUTTON_Click()
		{
			if (_currentActionContainer.ActionCheck())
			{
				i_Hide();
				_rejectionLabel.enabled = false;
			}
			else
			{
				var color = _rejectionLabel.color;
				color.a = 1f;
				_rejectionLabel.color = color;
				_rejectionLabel.enabled = true;
			}
		}

        public void OnLocaleChanged(LocalizationLanguage language)
		{
			if (_isShowing) LocalizeStrings();
		}
    }
}
