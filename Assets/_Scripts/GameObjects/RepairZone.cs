using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class RepairZone : PlayerTrigger
	{
        [SerializeField] private float _repairTime = 3f, _refuelTime = 3f;
        private bool _makeRepairs = false, _makeRefuel = false;
		private IntegrityModule _integrityModule;
		private FuelModule _fuelModule;

        protected override void Awake()
        {
            base.Awake();
            OnPlayerExitEvent += OnPlayerExit;
        }

        private void FixedUpdate()
        {
            if (IsPlayerInside)
            {
                float t = Time.fixedDeltaTime;
                if (_makeRefuel) _fuelModule.Refuel(t / _refuelTime);
                if (_makeRepairs) _integrityModule.Repairs(t / _repairTime);
            }
        }

        protected override void OnPlayerEnter(PlayerController player)
        {
            base.OnPlayerEnter(player);
            var vehicle = player.ActiveVehicle;
            if (!vehicle.TryGetFuelModule(out _fuelModule))
            {
                _fuelModule = null;
                _makeRefuel= false;
            }
            else
            {
                _makeRefuel = _refuelTime > 0f;
            }
            if (!vehicle.TryGetIntegrityModule(out _integrityModule))
            {
                _integrityModule = null;
                _makeRepairs = false;
            }
            else
            {
                _makeRepairs = _repairTime > 0f;
            }
        }

        private void OnPlayerExit() {
            _integrityModule= null;
            _fuelModule= null;
            _makeRepairs = false;
            _makeRefuel = false;
        }
    }
}
