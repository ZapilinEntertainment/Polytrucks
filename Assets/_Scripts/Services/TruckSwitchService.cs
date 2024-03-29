using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public class TruckSwitchService : ISwitchService
    {
        private IPlayerDataAgent _playerData;
        private TruckShowModule _showModule;
        private PlayerController _playerController;        
        private TruckSpawnService _truckSpawnService;
        private CachedVehiclesService _cachedVehiclesManager;
        public TruckSwitchService(PlayerController playerController, TruckSpawnService truckSpawnService, CachedVehiclesService cachedVehiclesManager,
            SignalBus signalBus,IAccountDataAgent accountDataAgent)
        {
            _playerController = playerController;
            _truckSpawnService = truckSpawnService;
            _cachedVehiclesManager = cachedVehiclesManager;
            _playerData = accountDataAgent.PlayerDataAgent;

            _showModule = new TruckShowModule(this);

            signalBus.Subscribe<GarageOpenedSignal>(OnGarageOpened);
            signalBus.Subscribe<GarageClosedSignal>(OnGarageClosed);
        }

        private void OnGarageOpened()
        {
            _showModule.OnPlayerTruckShown(_playerController.ActiveVehicle as Truck);
        }
        private void OnGarageClosed()
        {
            _showModule.ReturnToPlayerTruck();
            _truckSpawnService.CheckForTrailer(_playerController.ActiveVehicle as Truck);
        }

        public void SetPlayerVisibility(bool x)
        {
            _showModule.PlayerTruck.SetVisibility(x);
        }
        public bool IsTruckUnlocked(TruckID id) => _playerData.IsTruckUnlocked(id);
        public Truck GetTruck(TruckID id, VirtualPoint point)
        {
            var truck = _truckSpawnService.CreateTruck(id);
            truck.Teleport(point);
            return truck;
        }
        public GameObject GetPlaceholder(VirtualPoint point)
        {
            var placeholder = _truckSpawnService.CreatePlaceholder();
            placeholder.transform.SetPositionAndRotation(point.Position, Quaternion.LookRotation((Camera.main.transform.position - point.Position).normalized, Vector3.up));
            return placeholder;
        }
        public void CacheTruck(Truck truck) => _cachedVehiclesManager.CacheTruck(truck);
        public void CachePlaceholder(GameObject placeholder) => _cachedVehiclesManager.CachePlaceholder(placeholder);

        public Truck ShowTruck(TruckID truckID, VirtualPoint point, bool markAsPlayers = false) => _showModule.SwitchToTruck(truckID, point, markAsPlayers);

        public bool TrySwitchToTruck(TruckID truckID, out TruckSwitchReport report)
        {
            if (!_playerData.TrySwitchTruck(truckID, out report)) return false;
            else
            {
                var truck = ShowTruck(truckID, _playerController.ActiveVehicle.FormVirtualPoint(), true);
                _playerController.ChangeActiveVehicle(truck);
                return true;
            }
        }
    }
}
