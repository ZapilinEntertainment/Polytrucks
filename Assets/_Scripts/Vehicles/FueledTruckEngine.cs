using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class FueledTruckEngine : TruckEngine
	{
		protected readonly FuelModule _fuelModule;
		public FueledTruckEngine(FuelModule fuelModule, TruckConfig config, AxisControllerBase axis) : base (config, axis)
		{
			_fuelModule= fuelModule;
		}

        protected override bool CanAccelerate => base.CanAccelerate & _fuelModule.HaveFuel;
    }
}
