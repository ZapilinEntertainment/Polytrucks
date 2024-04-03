using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class PlayerVehicleController : IVehicleController
	{
        public bool AreControlsLocked => _controlsLocker.IsLocked;
        public int GetColliderLayer() => GameConstants.GetDefinedLayer(DefinedLayer.Player);
        public Vehicle ActiveVehicle { get; private set; }
        public Action OnLoseControlsEvent { get; set ; }
        public Action OnRestoreControlsEvent { get; set ; }
        public Action<Vehicle> OnActiveVehicleChangedEvent;

        private PlayerController _player;
        private Locker _controlsLocker;

        public PlayerVehicleController(Vehicle presetVehicle, PlayerController player, Locker controlsLocker)
        {
            ActiveVehicle = presetVehicle;
            
            _player = player;
            _controlsLocker = controlsLocker;
            _controlsLocker.OnLockStartEvent += OnControlsLocked;
            _controlsLocker.OnLockEndEvent += OnRestoreControls;

            if (ActiveVehicle != null) ActiveVehicle.AssignVehicleController(this);
        }

        #region IVehicleController
        public void Move(Vector2 dir)
        {
            if (_controlsLocker.IsLocked) return;
            ActiveVehicle.Move(dir);
        }
        public void ChangeMoveState(PlayerMoveStateType state, bool forced)
        {
            if (_controlsLocker.IsLocked && !forced) return;
            if (state == PlayerMoveStateType.Gas) ActiveVehicle.Gas();
            else
            {
                if (state == PlayerMoveStateType.Reverse) ActiveVehicle.Reverse();
                else ActiveVehicle.ReleaseGas();
            }
        }
        public void SetSteer(float steer)
        {
            if (_controlsLocker.IsLocked) return;
            ActiveVehicle.Steer(steer);
        }
        public void SetBrake(bool x)
        {
            if (x) ActiveVehicle.Brake();
            else ActiveVehicle.ReleaseBrake();
        }

        public void PhysicsLock(Rigidbody point, out int id)
        {
            if (_player.TryLockControls(out id)) ActiveVehicle.PhysicsLock(point);
        }
        public void RemovePhysicsLock(Rigidbody point, int id)
        {
            _player.UnlockControls(id);
            ActiveVehicle.PhysicsUnlock(point);
        }

        public void Stabilize() => ActiveVehicle.Stabilize();

        #region trading
        public void OnItemSold(SellOperationContainer info) => _player.OnItemSold(info);
        public void OnItemCompositionChanged() => _player.OnVehicleStorageContentChangedEvent?.Invoke();
        #endregion
        #endregion
        private void OnControlsLocked()
        {
            ReleaseControls();
            OnLoseControlsEvent?.Invoke();
        }
        private void OnRestoreControls()
        {
            OnRestoreControlsEvent?.Invoke();
        }
        public void ReleaseControls()
        {
            ChangeMoveState(PlayerMoveStateType.Idle, true);
            SetSteer(0f);
            SetBrake(true);
        }
        public void ChangeActiveVehicle(Vehicle vehicle)
        {
            if (ActiveVehicle != null)
            {
                ActiveVehicle.PhysicsUnlock();
                ActiveVehicle.AssignVehicleController(null);
            }
            OnLoseControlsEvent?.Invoke();
            ActiveVehicle= vehicle;
            ActiveVehicle.AssignVehicleController(this);
            OnRestoreControlsEvent?.Invoke();
            OnActiveVehicleChangedEvent?.Invoke(ActiveVehicle);
        }
    }
}
