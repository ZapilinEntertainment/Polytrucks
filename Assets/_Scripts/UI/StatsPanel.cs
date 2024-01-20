using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace ZE.Polytrucks {
	public class StatsPanel : MonoBehaviour, IDynamicLocalizer
	{
		[SerializeField] private ParameterLine[] _parameterLines;
		[SerializeField] private TMP_Text _truckNameLabel, _capacityParameterLabel, _capacityParameterValue;
		private TruckID _showingTruck = TruckID.Undefined;
		private Localization _localization;		

		[Inject]
		public void Inject(Localization localization)
		{
			_localization = localization;
			_localization.Subscribe(this);
		}

		public void OnLocaleChanged(LocalizationLanguage language) => FillStrings();
		private void FillStrings()
		{
			if (isActiveAndEnabled)
			{
				_capacityParameterLabel.text = _localization.GetParameterName(TruckParameterType.Capacity);
				_truckNameLabel.text = _localization.GetTruckName(_showingTruck);
			}
		}

        public void Show(TruckConfig truckConfig)
		{
			_showingTruck = truckConfig.TruckID;
			FillLine(0, TruckParameterType.MaxSpeed);
			FillLine(1, TruckParameterType.Acceleration);
			FillLine(2, TruckParameterType.Mass);
			FillLine(3, TruckParameterType.Passability);
			FillStrings();
			_capacityParameterValue.text = truckConfig.GetParameterValue(TruckParameterType.Capacity).ToString(); // todo: add upgrade val by another color

			void FillLine(int index, TruckParameterType parameter)
			{
				float maxVal = parameter.GetMaxValue();
				_parameterLines[index].Setup(parameter, truckConfig.GetParameterValue(parameter) / maxVal); // todo: add upgrade val
			}
		}
	}
}
