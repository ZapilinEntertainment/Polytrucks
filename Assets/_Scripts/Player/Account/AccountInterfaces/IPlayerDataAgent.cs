using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IPlayerDataAgent
	{
        public int Money { get; }
        public TruckID ActiveTruckID { get; }
        public Experience Experience { get; }

        public void OnPlayerSoldItem(SellOperationContainer sellOperation);
        public void SubscribeToMoneyChange(Action<int> action);
        public bool TrySpendMoney(int x);
        public bool TrySwitchVehicle(TruckID truckID, out TruckSwitchReport errormsg);

        public VirtualPoint GetRecoveryPoint();
    }
}
