using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {    
	public sealed class PlayerController : SessionObject, IVehicleController, IColliderOwner
	{
        [SerializeField] private Vehicle _vehicle;
        private IAccountDataAgent _accountDataAgent;
        private Locker _controlsLocker = new Locker();
        public Vector3 Position { get; private set; }
        public VirtualPoint FormVirtualPoint() => _vehicle.FormVirtualPoint();
        public Vehicle ActiveVehicle => _vehicle;
        public Action OnItemCompositionChangedEvent;
        public Action<Vehicle> OnVehicleChangedEvent;


        #region IColliderOwner
        public bool HasMultipleColliders => _vehicle.CollidersHandler.HasMultipleColliders;
        public int GetID() => _vehicle.CollidersHandler.GetID();
        public int[] GetIDs() => _vehicle.CollidersHandler.GetIDs();
        #endregion

        [Inject]
        public void Inject(IAccountDataAgent accountDataAgent, ColliderListSystem collidersList)
        {
            _accountDataAgent= accountDataAgent;
            collidersList.AddPlayer(this);
        }

        private void Awake()
        {
            Position = _vehicle.Position;
            _vehicle.AssignVehicleController(this);            
        }
        private void ChangeCameraPoint(Transform t)
        {
            if (isActiveAndEnabled) _signalBus.Fire(new CameraViewPointSetSignal(t));
        }

        public override void OnSessionStart()
        {
            base.OnSessionStart();
            ChangeCameraPoint( _vehicle.CameraViewPoint);            
        }

        #region controls
        public void Move(Vector2 dir)
        {
            if (_controlsLocker.IsLocked) return;
            _vehicle.Move(dir);
        }
        public void ChangeMoveState(PlayerMoveStateType state)
        {
            if (_controlsLocker.IsLocked) return;
            switch (state)
            {
                case PlayerMoveStateType.Gas: _vehicle.Gas(); break;
                case PlayerMoveStateType.Brake: _vehicle.Brake(); break;
                case PlayerMoveStateType.Reverse: _vehicle.Reverse(); break;
                default: _vehicle.ReleaseGas(); break;
            }
        }
        public void SetSteer(float steer)
        {
            if (_controlsLocker.IsLocked) return;
            _vehicle.Steer(steer);
        }
        
        #endregion
        private void LateUpdate()
        {
            Position = _vehicle.Position;
        }

        #region world positioning
        public void Stabilize() => _vehicle.Stabilize();
        public void Teleport(VirtualPoint point)
        {
            _vehicle.Teleport(point);
            Position = _vehicle.Position;
        }
        public void PhysicsLock(Rigidbody point, out int id)
        {
            if (TryLockControls(out id))  _vehicle.PhysicsLock(point);
        }
        private void ReleaseControls()
        {
            ChangeMoveState(PlayerMoveStateType.Idle);
            SetSteer(0f);
        }
        public bool TryLockControls(out int id)
        {
            ReleaseControls();
            
            id = _controlsLocker.CreateLock();            
            return true;
        }
        public void RemovePhysicsLock(Rigidbody point, int id)
        {
            UnlockControls(id);
            _vehicle.PhysicsUnlock(point);
        }
        public void UnlockControls(int id) => _controlsLocker.DeleteLock(id);
        public IReadOnlyCollection<Vector3> GetPlayerBounds() => _vehicle.GetVehicleBounds();
        #endregion

        #region trading
        public void OnItemSold(SellOperationContainer info) => _accountDataAgent.PlayerDataAgent.OnPlayerSoldItem(info);
        public bool CanFulfillContract(TradeContract contract) => _vehicle.CanFulfillContract(contract);
        public bool TryLoadCargo(VirtualCollectable item, int count) => _vehicle.TryLoadCargo(item, count);
        public TradeContract FormCollectContract() => _vehicle.FormCollectContract();       
        public void OnItemCompositionChanged()
        {
            OnItemCompositionChangedEvent?.Invoke();
        }

        #endregion

        #region player options
        public void ChangeActiveVehicle(Vehicle vehicle)
        {
            if (_vehicle != null)
            {
                _controlsLocker.ClearAllLocks();
                ReleaseControls();

                _vehicle.AssignVehicleController(null);
                _vehicle.PhysicsUnlock();
            }
            _vehicle = vehicle;
            _vehicle.AssignVehicleController(this);
            ChangeCameraPoint(_vehicle.CameraViewPoint);
            Position = _vehicle.Position;
            OnVehicleChangedEvent?.Invoke(vehicle);
        }
        #endregion
    }
}
