using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class MassChanger : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;
		private bool _initialMassSaved = false;
		private float _initialMass = 1;
		private Storage _storage;

		public void Setup(Storage storage)
		{
			if (_storage != null)
			{
				_storage.OnStorageCompositionChangedEvent -= RecalculateMass;
				_storage = null;
			}
			if (!_initialMassSaved)
			{
				_initialMass = _rigidbody.mass;
				_initialMassSaved = true;
			}
			_storage = storage;
            _storage.OnStorageCompositionChangedEvent += RecalculateMass;
			RecalculateMass();
		}

		private void RecalculateMass()
		{
			_rigidbody.mass = _initialMass + _storage.CargoMass;
		}
        private void OnDestroy()
        {
            if (_storage != null)
			{
				_storage.OnStorageCompositionChangedEvent -= RecalculateMass;
			}
        }
    }
}
