using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public enum TrailerID : byte { NoTrailer, FarmerTrailer}
	public sealed class Trailer : MonoBehaviour, ITrailerConnectionPoint, ICachableVehicle, ITeleportable
	{
		
        [SerializeField] private MassChanger _massChanger;
        [SerializeField] private Truck _truck;
		[SerializeField] private ConfigurableJoint _joint;
        [field: SerializeField] public TrailerID TrailerID { get; private set; } = TrailerID.NoTrailer;
        [field:SerializeField] public VisualStorageSettings StorageSettings { get; private set; }
		[field:SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }


        private bool _storageCreated = false;
		private ITrailerConnectionPoint _activeConnectionPoint;
		private StorageVisualizer _visualizer;
		private Storage _storage;
		private StorageVisualizer.Factory _visualizerFactory;

		public bool IsStorageCreated => _storageCreated;
		public bool IsTeleporting { get; private set; } = false;
        public VirtualPoint CalculateTrailerPosition(float distance)
        {
            var rotation = Rigidbody.rotation;
            return new VirtualPoint(Rigidbody.position + rotation * (distance * Vector3.back), rotation);
        }

        [Inject]
		public void Inject(StorageVisualizer.Factory factory)
		{
			_visualizerFactory= factory;
		}

		public Storage GetStorage()
		{
			if (!_storageCreated)
			{
				_storage = new Storage(StorageSettings.Capacity);
				_visualizer = _visualizerFactory.Create();
                _visualizer.Setup(_storage, StorageSettings);
				_storageCreated = true;
			}
			return _storage;
		}
		public bool TryGetStorage(out Storage storage)
		{
			if (_storageCreated)
			{
				storage = _storage;
				return true;
			}
			else
			{
                storage = null;
				return false;
			}
		}

        private void Start()
        {
			if (!_storageCreated) GetStorage();
			if (_massChanger != null)
			{
				_massChanger.Setup(_storage);
			}
			if (_truck != null)
			{
				_truck.TrailerConnector.AddTrailer(this);				
			}
        }
		public void OnTrailerConnected(ITrailerConnectionPoint connector)
		{
			_activeConnectionPoint = connector;
			SetJointLock(false);
			_joint.connectedBody = _activeConnectionPoint.Rigidbody;
			var point = _activeConnectionPoint.CalculateTrailerPosition(_joint.anchor.z - _joint.connectedAnchor.z);
			Teleport(point, null);
        }
		private void OnTeleported()
		{
			if (_activeConnectionPoint != null)
			{
				SetJointLock(true);
			}
		}
		private void SetJointLock(bool isLocked)
		{
            if (isLocked)
            {
                _joint.xMotion = ConfigurableJointMotion.Locked;
                _joint.yMotion = ConfigurableJointMotion.Locked;
                _joint.zMotion = ConfigurableJointMotion.Locked;
            }
            else
            {
                _joint.xMotion = ConfigurableJointMotion.Free;
                _joint.yMotion = ConfigurableJointMotion.Free;
                _joint.zMotion = ConfigurableJointMotion.Free;
            }
        }

		public void SetVisibility(bool x)
		{
			gameObject.SetActive(x);
			if (!x && _activeConnectionPoint != null)
			{
				_joint.connectedBody = null;
				_activeConnectionPoint = null;
			}
		}

		public void Teleport(VirtualPoint point, System.Action onTeleportCompleted)
		{
            SetJointLock(false);
            RigidbodyTeleportationService.Teleport(Rigidbody, point, OnTeleported + onTeleportCompleted);
        }

        private void OnDestroy()
        {
			_visualizer?.Dispose();
        }

        public class Factory : PlaceholderFactory< UnityEngine.Object, Trailer>
        {
        }
    }
}
