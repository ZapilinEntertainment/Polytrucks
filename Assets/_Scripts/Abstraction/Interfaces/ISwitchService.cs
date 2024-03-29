using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISwitchService 
	{
        public bool TrySwitchToTruck(TruckID truckID, out TruckSwitchReport report);
        public Truck ShowTruck(TruckID truckID, VirtualPoint point, bool markAsPlayers = false);
    }
}
