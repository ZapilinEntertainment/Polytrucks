using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class FuelModule :  ITrackableVehicleModule
	{
        protected IFuelConfiguration _fuelConfiguration;
        protected Vehicle _vehicle;

        public bool HaveFuel => Fuel > 0f;
        public float Fuel { get; protected set; }
        public float MeaningValue => Fuel / _fuelConfiguration.TankVolume;
        public System.Action OnModuleDisposedEvent { get; set; }

        public FuelModule(IFuelConfiguration fuelConfiguration, Vehicle vehicle,  float fuelCf = 1f)
        {
            _fuelConfiguration = fuelConfiguration;
            _vehicle = vehicle;
            Fuel = fuelCf * fuelConfiguration.TankVolume;
            _vehicle.OnVehicleDisposeEvent += OnDisposed;
        }

        public void Update(float t)
        {
            Fuel -= t * _fuelConfiguration.FuelConsumption * Mathf.Abs(_vehicle.GasValue);
            if (Fuel < 0f) Fuel = 0f;
        }

        private void OnDisposed()
        {
            OnModuleDisposedEvent?.Invoke();
        }
    }
}
