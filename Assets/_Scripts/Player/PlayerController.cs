using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {    
	public sealed class PlayerController : SessionObject, IVehicleController, IColliderOwner
	{
       

        [SerializeField] private Vehicle _vehicle;
        [SerializeField] private InputController _inputController;
        private IAccountDataAgent _accountDataAgent;
        private Locker _locker = new Locker();
        public Vector3 Position { get; private set; }
        public VirtualPoint FormVirtualPoint() => _vehicle.FormVirtualPoint();
        public InputController InputController => _inputController;
        public Vehicle ActiveVehicle => _vehicle;
        public Action OnItemCompositionChangedEvent;


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
            _inputController.Setup(this);
            _vehicle.AssignVehicleController(this);
        }

        public override void OnSessionStart()
        {
            base.OnSessionStart();
            Transform pointLink = _vehicle.CameraViewPoint;
            if (isActiveAndEnabled) _signalBus.Fire(new CameraViewPointSetSignal(pointLink));
        }

        #region controls
        public void Move(Vector2 dir)
        {
            if (_locker.IsLocked) return;
            _vehicle.Move(dir);
        }
        public void ChangeMoveState(PlayerMoveStateType state)
        {
            if (_locker.IsLocked) return;
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
            if (_locker.IsLocked) return;
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
        public bool TryLock(Transform point, out int id)
        {
            ChangeMoveState(PlayerMoveStateType.Idle);
            SetSteer(0f);
            Teleport(new VirtualPoint(point));
            id = _locker.CreateLock();            
            return true;
        }
        public void Unlock(int id) => _locker.DeleteLock(id);
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
    }
}
