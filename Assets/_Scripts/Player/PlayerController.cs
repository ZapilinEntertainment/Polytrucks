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
        private PlayerData _playerData;
        public static Vector3 Position { get; private set; }
        public VirtualPoint FormVirtualPoint() => _vehicle.FormVirtualPoint();
        public InputController InputController => _inputController;
        public Action OnItemCompositionChangedEvent;


        #region IColliderOwner
        public bool HasMultipleColliders => _vehicle.CollidersHandler.HasMultipleColliders;
        public int GetID() => _vehicle.CollidersHandler.GetID();
        public int[] GetIDs() => _vehicle.CollidersHandler.GetIDs();
        #endregion

        [Inject]
        public void Inject(PlayerData playerData, ColliderListSystem collidersList)
        {
            _playerData = playerData;
            collidersList.AddPlayer(this);
        }

        private void Awake()
        {
            Position = transform.position;
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
            _vehicle.Move(dir);
        }
        public void ChangeMoveState(PlayerMoveStateType state)
        {
            switch (state)
            {
                case PlayerMoveStateType.Gas: _vehicle.Gas(); break;
                case PlayerMoveStateType.Brake: _vehicle.Brake(); break;
                case PlayerMoveStateType.Reverse: _vehicle.Reverse(); break;
                default: _vehicle.ReleaseGas(); break;
            }
        }
        public void SetSteer(float steer) => _vehicle.Steer(steer);
        public void Stabilize() => _vehicle.Stabilize();
        #endregion
        private void LateUpdate()
        {
            if (GameSessionActive)  Position = _vehicle.Position;
        }

        public void Teleport(VirtualPoint point)
        {
            _vehicle.Teleport(point);
            Position = _vehicle.Position;
        }

        #region trading
        public void OnItemSold(SellOperationContainer info) => _playerData.OnPlayerSoldItem(info);
        public bool CanFulfillContract(TradeContract contract) => _vehicle.CanFulfillContract(contract);
        public TradeContract FormCollectContract() => _vehicle.FormCollectContract();
        public void OnItemCompositionChanged()
        {
            OnItemCompositionChangedEvent?.Invoke();
        }

        #endregion
    }
}
