using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class Garage : PlayerTrigger
	{
        [SerializeField] private Transform _modelPoint;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera _virtualCamera;
        private bool _isObserving = false;
        private Truck _playerTruck;
        private SignalBus _signalBus;
        private Truck.Factory _truckFactory;

        [Inject]
        public void Inject(SignalBus signalBus)
        {
            _signalBus= signalBus;
        }

        protected override void OnPlayerEnter(PlayerController player)
        {
            base.OnPlayerEnter(player);
            _playerTruck = player.ActiveVehicle as Truck;
            _signalBus.Fire(new GarageOpenedSignal(this));            
        }

        public void SetObservingStatus(bool x)
        { 
            if (x != _isObserving)
            {
                _isObserving = x;
                SetPlayerTruckVisibility(!x);
                _virtualCamera.enabled = x;
            }
        }
        private void SetPlayerTruckVisibility(bool x)
        {
            _player.ActiveVehicle.gameObject.SetActive(!x);
        }

        public Truck SpawnTruck(Truck prefab)
        {
            if (prefab.TruckID == _playerTruck.TruckID)
            {
                SetPlayerTruckVisibility(true);
                return _playerTruck;
            }
            else
            {
                SetPlayerTruckVisibility(false);
                var truck = _truckFactory.Create(prefab);
                truck.transform.SetPositionAndRotation(_modelPoint.position, _modelPoint.rotation);
                return truck;
            }
        }
    }
}
