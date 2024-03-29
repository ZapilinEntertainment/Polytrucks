using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {    
	public sealed class PlayerController : SessionObject, IColliderOwner
	{
        private IPlayerDataAgent _playerData;
        private PlayerVehicleController _vehicleController;
        private PlayerControlsLocker _controlsLocker;
        private ColliderListSystem _colliderListSystem;
        
        public Vector3 Position { get; private set; }
        public VirtualPoint FormVirtualPoint() => _vehicleController.ActiveVehicle.FormVirtualPoint();
        
        public IVehicleController VehicleController => _vehicleController;
        public Vehicle ActiveVehicle => VehicleController.ActiveVehicle;
        public Action OnVehicleStorageContentChangedEvent;
        public Action<Vehicle> OnVehicleChangedEvent;
       

        [Inject]
        public void Inject(IAccountDataAgent accountDataAgent, ColliderListSystem collidersList, SignalBus signalBus, TruckSpawnService truckSpawner)
        {
            _playerData = accountDataAgent.PlayerDataAgent;
            _controlsLocker = new PlayerControlsLocker(signalBus);

            Truck truck = truckSpawner.CreateTruck(_playerData.ActiveTruckID);
            _vehicleController = new PlayerVehicleController(truck, this, _controlsLocker);
            _vehicleController.OnActiveVehicleChangedEvent += OnVehicleChanged;
            truckSpawner.CheckForTrailer(truck);

            _colliderListSystem = collidersList;
        }
        private void OnVehicleChanged(Vehicle vehicle)
        {
            Position = vehicle.Position;
            if (isActiveAndEnabled)
            {
                _signalBus.Fire(new CameraViewPointSetSignal(vehicle.CameraViewPoint, vehicle.ViewSettings));
            }
            _colliderListSystem.AddPlayerColliders(this);
            OnVehicleChangedEvent?.Invoke(vehicle);
        }


        private void Start()
        {
            if (ActiveVehicle != null)
            {
                bool storageReady = true;
                if (ActiveVehicle is Truck)
                {
                    var truck = ActiveVehicle as Truck;
                    if (truck.TruckConfig.TrailerRequired && !truck.HaveTrailers)
                    {
                        storageReady = false;
                        truck.OnTrailerConnectedEvent += OnTrailerConnected;
                    }
                }
                if (storageReady) OnVehicleStorageReady();
                
                ActiveVehicle.Teleport(_playerData.GetRecoveryPoint(), () => OnVehicleChanged(ActiveVehicle));               
            }
        }
        private void OnVehicleStorageReady()
        {
            ActiveVehicle.VehicleStorageController.Storage.AddItems(_playerData.GetVehicleCargo(), out BitArray resultArray);
        }
        private void OnTrailerConnected()
        {
            (ActiveVehicle as Truck).OnTrailerConnectedEvent -= OnTrailerConnected;
            OnVehicleStorageReady();
        }

        private void LateUpdate()
        {
            if (ActiveVehicle != null) Position = ActiveVehicle.Position;
        }

        
        public void Teleport(VirtualPoint point)
        {
            if (ActiveVehicle != null)
            {
                ActiveVehicle.Teleport(point);
                Position = ActiveVehicle.Position;
            }
        }
       
       
        public bool TryLockControls(out int id)
        {
            id = _controlsLocker.CreateLock();            
            return true;
        }        
        public void UnlockControls(int id) => _controlsLocker.DeleteLock(id);
        public IReadOnlyCollection<Vector3> GetPlayerBounds() => ActiveVehicle.GetVehicleBounds();
        

        #region trading
        public void OnItemSold(SellOperationContainer info) => _playerData.OnPlayerSoldItem(info);
        public bool CanFulfillContract(TradeContract contract)
        {
            if (ActiveVehicle.TryGetStorage(out var storage) && storage.CanFulfillContract(contract)) return true;
            else return false;
        }
        public bool TryLoadCargo(VirtualCollectable item, int count)
        {
            if (ActiveVehicle.TryGetStorage(out var storage) && storage.TryLoadCargo(item, count)) return true;
            else return false;
        }
        public TradeContract FormCollectContract() => ActiveVehicle.FormCollectContract();

        #endregion

        #region IColliderOwner
        public bool HaveMultipleColliders => ActiveVehicle.HaveMultipleColliders;
        public int GetColliderID() => ActiveVehicle.GetColliderID();
        public int[] GetColliderIDs() => ActiveVehicle.GetColliderIDs();
        #endregion

        #region player options
        public void ChangeActiveVehicle(Vehicle vehicle)
        {
            if (ActiveVehicle != null)
            {
                _colliderListSystem.RemovePlayerCollider(this);
            }
           _vehicleController.ChangeActiveVehicle(vehicle); 
        }
        #endregion
    }
}
