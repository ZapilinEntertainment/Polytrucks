using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZE.Polytrucks {
	public class ParameterLine : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] protected Image _valueImage, _upgradedImage;
		[SerializeField] protected TMPro.TMP_Text _parameterLabel;
		protected TruckParameterType _parameterType;
		protected Localization _localization;

		[Inject]
		public void Inject(Localization localization)
		{
			_localization = localization;
			_localization.Subscribe(this);
		}

		public void Setup(TruckParameterType parameterType, float baseValue, float upgradedValue = 0f)
		{
			_valueImage.fillAmount = baseValue;
			if (upgradedValue == 0f) _upgradedImage.enabled = false;
			else
			{
				_upgradedImage.fillAmount = upgradedValue;
				_upgradedImage.enabled = true;
			}
			_parameterType = parameterType;
		}
		private void SetStringValue()
		{
			if (isActiveAndEnabled) _parameterLabel.text = _localization.GetParameterName(_parameterType);
		}

		public void OnLocaleChanged(LocalizationLanguage language) => SetStringValue();
        private void OnDestroy()
        {
			_localization?.Unsubscribe(this);
        }
    }
}
