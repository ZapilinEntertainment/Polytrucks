using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

    public enum VehicleType : byte
    {
        None = 0, Truck
    }
	public abstract class Vehicle : SessionObject, IColliderOwner
	{
		[SerializeField] private Transform _cameraViewPoint;
        [SerializeField] protected TradeCollidersHandler _collidersHandler;
        public abstract StorageController VehicleStorageController { get; }
        public IVehicleController VehicleController { get; protected set; }
        public TradeCollidersHandler CollidersHandler => _collidersHandler;
        public Action OnVehicleDisposeEvent;
        public Action<IVehicleController> OnVehicleControllerChangedEvent;
        
        abstract public float GasValue { get; }
        abstract public float SteerValue { get; }
        abstract public float Speed { get; }
        abstract public float SpeedPc { get; }  
        abstract public Vector3 Position { get; }
        abstract public VirtualPoint FormVirtualPoint();
		public Transform CameraViewPoint => _cameraViewPoint;

		public abstract void Move(Vector2 dir);
        public abstract void Gas();
        public abstract void Brake();
        public abstract void Reverse();
        public abstract void ReleaseGas();
        public abstract void ReleaseBrake();
        public abstract void Steer(float x);

        #region world positioning
        public abstract void Teleport(VirtualPoint point);
        public abstract void Stabilize();
        public abstract void PhysicsLock(Rigidbody point);
        public abstract void PhysicsUnlock(Rigidbody point = null);
        public abstract void RecoveryAt(RecoveryPoint point);       

        virtual public IReadOnlyCollection<Vector3> GetVehicleBounds() => _collidersHandler.GetBounds();
        #endregion

        public void AssignVehicleController(IVehicleController controller) { VehicleController = controller; OnVehicleControllerChangedEvent?.Invoke(VehicleController); }

        #region storage
        public abstract void ClearCargo(bool destroy = true);
        public abstract bool CanFulfillContract(TradeContract contract);
        public abstract int LoadCargo(VirtualCollectable item, int count);
        public abstract bool TryLoadCargo(VirtualCollectable item, int count);
       
        public abstract TradeContract FormCollectContract();
        #endregion
        #region IColliderOwner
        public bool HasMultipleColliders => CollidersHandler.HasMultipleColliders;
        public int GetColliderID() => CollidersHandler.GetColliderID();
        public int[] GetColliderIDs() => CollidersHandler.GetColliderIDs();
        #endregion

        private void OnDestroy()
        {
            OnVehicleDisposeEvent?.Invoke();            
        }
    }
}
