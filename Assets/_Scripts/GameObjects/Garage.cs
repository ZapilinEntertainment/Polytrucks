using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class Garage : PlayerTrigger
	{
        [SerializeField] private Transform _modelPoint;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _virtualCamera;
        private bool _waitUntilPlayerLeave = false;
        private Truck _playerTruck;
        private SignalBus _signalBus;
        private TruckSwitchService _garageService;
        private int? _playerLockId = null;
        public VirtualPoint ModelPoint => new VirtualPoint(_modelPoint);

        [Inject]
        public void Inject(SignalBus signalBus, TruckSwitchService garageService)
        {
            _signalBus= signalBus;
            _garageService= garageService;
        }
        private void Start()
        {
            OnPlayerExitEvent += OnPlayerExit;
        }

        protected override void OnPlayerEnter(PlayerController player)
        {
            if (_waitUntilPlayerLeave) return;
            base.OnPlayerEnter(player);
            if (_playerLockId != null) return;
            int lockValue;
            if (player.TryLockControls(out lockValue))
            {
                _waitUntilPlayerLeave = true;
                _playerLockId = lockValue;
                var vehicle = player.ActiveVehicle;
                vehicle.Teleport(ModelPoint);
                vehicle.ReleaseGas();
                vehicle.Brake();
                _playerTruck = vehicle as Truck;                
                _signalBus.Fire(new GarageOpenedSignal(this));                
            }
        }
        private void OnPlayerExit()
        {
            _waitUntilPlayerLeave = false;
        }

        public void SetObservingStatus(bool isActive)
        {
            _virtualCamera.enabled = isActive;
            if (isActive == false && _playerLockId != null)
            {
                _player.UnlockControls(_playerLockId.Value);
                _playerLockId = null;
            }
            SetTrailersVisibility(!isActive);
        }
        private void SetTrailersVisibility(bool x)
        {
            if (_playerTruck != null && _playerTruck.HaveTrailers)
            {
                _playerTruck.TrailerConnector.SetTrailersVisibility(x);
            }
        }
    }
    
}
