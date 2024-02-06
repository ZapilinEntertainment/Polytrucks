using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

    public enum VehicleType : byte
    {
        None = 0, Truck
    }
	public abstract class Vehicle : SessionObject, IColliderOwner, ITeleportable
	{
		[SerializeField] protected Transform _cameraViewPoint;
        [SerializeField] protected CollidersHandler _collidersHandler;

        public bool IsVisible { get; private set; } = true;
        public abstract StorageController VehicleStorageController { get; }
        public IVehicleController VehicleController { get; protected set; }
        public CollidersHandler CollidersHandler => _collidersHandler;
        public Action OnVehicleDisposeEvent;
        public Action<bool> OnVisibilityChangedEvent;
        public Action<IVehicleController> OnVehicleControllerChangedEvent;

        virtual public bool IsTeleporting { get; protected set; } = false;
        abstract public float GasValue { get; }
        abstract public float SteerValue { get; }
        abstract public float Speed { get; }
        abstract public float SpeedPc { get; }  
        abstract public Vector3 Position { get; }
        abstract public VirtualPoint FormVirtualPoint();
		public Transform CameraViewPoint => _cameraViewPoint;
        public float GetSpeedEffectOffsetCf()
        {
            if (CameraViewPoint != null && CameraViewPoint.TryGetComponent<ViewPointCorrector>(out var corrector))
            {
                return corrector.SpeedEffectOffsetCf;
            }
            else return 1f;
        }

		public abstract void Move(Vector2 dir);
        public abstract void Gas();
        public abstract void Brake();
        public abstract void Reverse();
        public abstract void ReleaseGas();
        public abstract void ReleaseBrake();
        public abstract void Steer(float x);

        #region world positioning
        public abstract void Teleport(VirtualPoint point, Action onTeleportComplete = null);
        public abstract void Stabilize();
        public abstract void PhysicsLock(Rigidbody point);
        public abstract void PhysicsUnlock(Rigidbody point = null);
        public abstract void RecoveryAt(RecoveryPoint point);       

        virtual public IReadOnlyCollection<Vector3> GetVehicleBounds() => _collidersHandler.GetBounds();
        #endregion

        #region modules
        public void AssignVehicleController(IVehicleController controller) { 
            VehicleController = controller;
            if (CollidersHandler != null) CollidersHandler.SetLayer(controller?.GetColliderLayer() ?? GameConstants.GetDefinedLayer(DefinedLayer.Default));
            OnVehicleControllerChangedEvent?.Invoke(VehicleController); 
        }

        public virtual bool TryGetStorage(out IStorage storage)
        {
            storage = null;
            return false;
        }
        public virtual bool TryGetFuelModule(out FuelModule module)
        {
            module = null;
            return false;
        }
        public virtual bool TryGetIntegrityModule(out IntegrityModule module)
        {
            module = null;
            return false;
        }

        #endregion
        #region storage
        public abstract void ClearCargo(bool destroy = true);       
        public abstract TradeContract FormCollectContract();
        #endregion
        #region IColliderOwner
        public bool HaveMultipleColliders => CollidersHandler.HaveMultipleColliders;
        public int GetColliderID() => CollidersHandler.GetColliderID();
        public int[] GetColliderIDs() => CollidersHandler.GetColliderIDs();
        #endregion

        public void SetVisibility(bool x)
        {
            _collidersHandler.SetColliderActivity(x);
            gameObject.SetActive(x);
            IsVisible= x;
            if (IsVisible)
            {
                ReleaseBrake();
                OnVisibilityChangedEvent?.Invoke(true);
            }
            else
            {
                ReleaseGas();
                Brake();
                OnVisibilityChangedEvent?.Invoke(false);
            }
        }


        public override void OnObjectDispose()
        {
            OnVehicleDisposeEvent?.Invoke();
        }
    }
}
