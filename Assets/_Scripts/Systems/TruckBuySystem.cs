using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class TruckBuySystem
	{
		private int _defaultCost = 1000;
		private UIManager _uiManager;
		private IPlayerDataAgent _playerData;
		private Dictionary<TruckID, int> _costs = new();
		public TruckBuySystem(EconomicSettings economicSettings, UIManager uimanager, IAccountDataAgent accountData)
		{
			var truckCosts = economicSettings.TruckCosts;
			if (truckCosts.Length > 0)
			{
				foreach (var truck in truckCosts)
				{
					_costs.Add(truck.TruckID, truck.Cost);
				}
			}

			_uiManager = uimanager;
			_playerData = accountData.PlayerDataAgent;
		}
		private int GetTruckCost(TruckID id)
		{
			if (_costs.TryGetValue(id, out var val)) return val;
			else return _defaultCost;
		}

		public void OnTruckDealStarted(TruckID truck, Vector3 worldPos, float radius)
		{
			_uiManager.ShowActionPanel(
				new TruckBuyActionContainer(
					id: truck,
					costLabel: GetTruckCost(truck).ToString(),
					mainLabel: LocalizedString.Ask_BuyTruck,
					rejectionLabel: LocalizedString.NotEnoughMoney,
					resultFunc: () => TryBuyTruck(truck),
					worldPos: worldPos,
					radius: radius
					)				
				);
		}
		public bool TryBuyTruck(TruckID id)
		{
			if (_playerData.TrySpendMoney(GetTruckCost(id))) {
				_playerData.UnlockTruck(id); 
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
