using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public enum TrailerID : byte { NoTrailer, FarmerTrailer, RigTrailer}
	public sealed class Trailer : MonoBehaviour, ITrailerConnectionPoint, ICachableVehicle, ITeleportable
	{
        [SerializeField] private MassChanger _massChanger;
        [SerializeField] private Truck _truck;
        [SerializeField] private TrailerJointConfig _trailerJointConfig;
		[field: SerializeField] public float ConnectDistance { get; private set; } = 5.4f;
        [field: SerializeField] public TrailerID TrailerID { get; private set; } = TrailerID.NoTrailer;
        [field:SerializeField] public VisualStorageSettings StorageSettings { get; private set; }
		[field:SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }


        private bool _storageCreated = false;		
		private ITrailerConnectionPoint _activeConnectionPoint;
        private ConfigurableJoint _joint;
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
			PositionAndConnectTrailer();
        }	
		private void PositionAndConnectTrailer()
		{
            var point = _activeConnectionPoint.CalculateTrailerPosition(ConnectDistance);
            Teleport(point);
        }

		private void RestoreJoint()
		{
            if (_joint == null && _activeConnectionPoint?.Rigidbody != null)
            {
               _joint = gameObject.AddComponent<ConfigurableJoint>();

				_joint.autoConfigureConnectedAnchor = false;
                _joint.connectedBody = _activeConnectionPoint.Rigidbody;
                _trailerJointConfig.FillValuesTo(_joint);
                _joint.xMotion = ConfigurableJointMotion.Locked;
                _joint.yMotion = ConfigurableJointMotion.Locked;
                _joint.zMotion = ConfigurableJointMotion.Locked;
                _joint.angularXMotion = ConfigurableJointMotion.Limited;
                _joint.angularYMotion = ConfigurableJointMotion.Limited;
				_joint.angularZMotion = _trailerJointConfig.AngularZMotion;
            }
        }
		private void ClearJoint()
		{
			if (_joint != null)
			{				
				Destroy(_joint);
                _joint = null;
            }
        }

		public void SetVisibility(bool x)
		{			
			if (x)
			{
				PositionAndConnectTrailer();
			}
			else
			{
				ClearJoint();
			}
            gameObject.SetActive(x);
        }

		public void Teleport(VirtualPoint point, System.Action onTeleportCompleted = null)
		{
			ClearJoint();
            RigidbodyTeleportationService.Teleport(Rigidbody, point, OnTeleported + onTeleportCompleted);

        }
        private void OnTeleported()
        {
			RestoreJoint();
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
