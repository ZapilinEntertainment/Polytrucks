using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class GaragePanel : MonoBehaviour
	{
		[SerializeField] private VisualItemsListController _buttonsController;
		private Garage _observingGarage;
		public void Open(Garage garage)
		{
			_observingGarage= garage;
			gameObject.SetActive(true);
		}
		public void Close()
		{
            gameObject.SetActive(false);
        }
	}
}
