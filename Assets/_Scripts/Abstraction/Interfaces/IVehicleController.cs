using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IVehicleController 
	{
        public void OnItemSold(SellOperationContainer info);
    }
}