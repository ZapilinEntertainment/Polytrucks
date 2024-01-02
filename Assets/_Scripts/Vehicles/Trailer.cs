using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class Trailer : MonoBehaviour, ITrailerConnectionPoint
	{
        [SerializeField] private MassChanger _massChanger;
        [SerializeField] private Truck _truck;
		[SerializeField] private ConfigurableJoint _joint;
        [field:SerializeField] public StorageVisualSettings StorageSettings { get; private set; }
		[field:SerializeField] public Collider Collider { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }


        private bool _storageCreated = false;
		private Storage _storage;
		private StorageVisualizer.Factory _visualizerFactory;

		public bool IsStorageCreated => _storageCreated;
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
				var visualizer = _visualizerFactory.Create();
				visualizer.Setup(_storage, StorageSettings);
				_storageCreated = true;
			}
			return _storage;
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
				_truck.AddTrailer(this);				
			}
        }
		public void OnTrailerConnected(ITrailerConnectionPoint connector)
		{
			var point = connector.CalculateTrailerPosition(_joint.anchor.z - _joint.connectedAnchor.z);
			transform.position = point.Position;
			transform.rotation = point.Rotation;
			
			Rigidbody.ResetInertiaTensor();
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;

            _joint.connectedBody = connector.Rigidbody;
			_joint.xMotion = ConfigurableJointMotion.Locked;
			_joint.yMotion = ConfigurableJointMotion.Locked;
			_joint.zMotion= ConfigurableJointMotion.Locked;
        }

        public class Factory : PlaceholderFactory<Trailer>
        {
        }
    }
}
