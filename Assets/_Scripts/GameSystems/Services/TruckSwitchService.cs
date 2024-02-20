using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public class TruckSwitchService
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
        }

        private void SetPlayerVisibility(bool x)
        {
            _showModule.PlayerTruck.SetVisibility(x);
        }
        private bool IsTruckUnlocked(TruckID id) => _playerData.IsTruckUnlocked(id);
        private Truck GetTruck(TruckID id, VirtualPoint point)
        {
            var truck = _truckSpawnService.CreateTruck(id);
            truck.Teleport(point);
            return truck;
        }
        private GameObject GetPlaceholder(VirtualPoint point)
        {
            var placeholder = _truckSpawnService.CreatePlaceholder();
            placeholder.transform.SetPositionAndRotation(point.Position, Quaternion.LookRotation((Camera.main.transform.position - point.Position).normalized, Vector3.up));
            return placeholder;
        }
        private void CacheTruck(Truck truck) => _cachedVehiclesManager.CacheTruck(truck);
        private void CachePlaceholder(GameObject placeholder) => _cachedVehiclesManager.CachePlaceholder(placeholder);

        public Truck ShowTruck(TruckID truckID, VirtualPoint point) => _showModule.SwitchToTruck(truckID, point);

        public bool TrySwitchToTruck(TruckID truckID, out TruckSwitchReport report)
        {
            if (!_playerData.TrySwitchTruck(truckID, out report)) return false;
            else
            {
                var truck = ShowTruck(truckID, _playerController.ActiveVehicle.FormVirtualPoint());
                _playerController.ChangeActiveVehicle(truck);
                _truckSpawnService.CheckForTrailer(truck);
                return true;
            }
        }
       

        private enum TruckShowState { NoTruck, PlayerTruck, SelectedTruck, Placeholder}        
        private class TruckShowModule
        {
            private enum PlaceholderAction { DoNothing, HidePlaceholder, ShowPlaceholder }

            private bool _placeholderActive = false;
            private readonly TruckSwitchService _switchService;
            private GameObject _placeholder;
            public Truck PlayerTruck { get; private set; } = null;
            public Truck ShowingTruck { get; private set; } = null;
            public TruckShowState State { get; private set; } = TruckShowState.NoTruck;

            public TruckShowModule(TruckSwitchService switchService)
            {
                _switchService = switchService;
            }

            public void OnPlayerTruckShown(Truck playerTruck)
            {
                State = TruckShowState.PlayerTruck;
                PlayerTruck = ShowingTruck = playerTruck;
                Debug.Log(PlayerTruck == null);
            }
            public void ReturnToPlayerTruck()
            {
                if (State == TruckShowState.SelectedTruck)
                {
                    _switchService.CacheTruck(ShowingTruck);                    
                }
                else
                {
                    if (State == TruckShowState.Placeholder)
                    {
                        _switchService.CachePlaceholder(_placeholder);
                    }
                }
                PlayerTruck.SetVisibility(true);
                ShowingTruck = PlayerTruck;
                State = TruckShowState.PlayerTruck;
            }

            public Truck SwitchToTruck(TruckID id, VirtualPoint point)
            {
                TruckShowState nextState;
                if (id == TruckID.Undefined) nextState = TruckShowState.Placeholder;
                else
                {
                    if (id == PlayerTruck.TruckID) nextState = TruckShowState.PlayerTruck;
                    else nextState= TruckShowState.SelectedTruck;
                }


                PlaceholderAction placeholderAction = PlaceholderAction.DoNothing;
                if (nextState != State)
                {
                    switch (State)
                    {
                        case TruckShowState.PlayerTruck:
                            {
                                _switchService.SetPlayerVisibility(false);
                                break;
                            }
                        case TruckShowState.SelectedTruck:
                            {
                                if (_placeholderActive)
                                {
                                    placeholderAction = PlaceholderAction.HidePlaceholder;
                                }
                                else
                                {
                                    _switchService.CacheTruck(ShowingTruck);
                                }
                                break;
                            }
                        case TruckShowState.Placeholder:
                            {
                                placeholderAction = PlaceholderAction.HidePlaceholder;
                                break;
                            }
                    }
                    switch (nextState)
                    {
                        case TruckShowState.PlayerTruck:
                            {
                                ShowingTruck = PlayerTruck;
                                _switchService.SetPlayerVisibility(true);
                                break;
                            }
                        case TruckShowState.SelectedTruck:
                            {
                                if (_switchService.IsTruckUnlocked(id))
                                {
                                    ShowingTruck = _switchService.GetTruck(id, point);
                                    ShowingTruck.SetVisibility(true);
                                }
                                else placeholderAction = PlaceholderAction.ShowPlaceholder;
                                break;
                            }
                        case TruckShowState.Placeholder:
                            {
                                placeholderAction = PlaceholderAction.ShowPlaceholder;                                
                                break;
                            }
                        case TruckShowState.NoTruck:
                            {
                                ShowingTruck = null;
                                break;
                            }
                    }

                    State = nextState;
                }

                switch (placeholderAction)
                {
                    case PlaceholderAction.HidePlaceholder:
                        {
                            _switchService.CachePlaceholder(_placeholder);
                            _placeholder = null;
                            _placeholderActive = false;
                            break;
                        }
                    case PlaceholderAction.ShowPlaceholder:
                        {
                            ShowingTruck = null;
                            _placeholder = _switchService.GetPlaceholder(point);
                            _placeholder.SetActive(true);
                            _placeholderActive = true;
                            break;
                        }
                }

                return ShowingTruck;
            }
        }
    }
}
