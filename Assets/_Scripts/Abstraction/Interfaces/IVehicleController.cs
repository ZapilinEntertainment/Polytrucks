using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    public interface IVehicleController
    {
        public bool AreControlsLocked { get;  }
        public Vehicle ActiveVehicle { get; }
        public Action OnLoseControlsEvent { get; set; }
        public Action OnRestoreControlsEvent { get; set; }

        public void Move(Vector2 dir);
        public void Stabilize();
        public void ChangeMoveState(PlayerMoveStateType state, bool forced = false);
        public void SetSteer(float x);
        public void SetBrake(bool x);

        public void PhysicsLock(Rigidbody point, out int id);
        public void RemovePhysicsLock(Rigidbody point, int id);

        public void OnItemSold(SellOperationContainer info);
        public void OnItemCompositionChanged();
    }
}
