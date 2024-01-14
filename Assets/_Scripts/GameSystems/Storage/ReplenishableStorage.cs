using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class ReplenishableStorage : ReplenishableStorageBase
	{
		[SerializeField] private StorageVisualSettings _storageSettings;
		[SerializeField] private CollectZone _collectZone;
		private StorageVisualizer.Factory _visualizerFactory;

		[Inject]
		public void Inject(StorageVisualizer.Factory factory) => _visualizerFactory = factory;

        protected override void Start()
        {
			var storage = PrepareStorage();
			AssignReceiver(storage);
			SpawnStartItems(storage);
        }
		virtual protected Storage PrepareStorage()
		{
            var storage = new Storage(_storageSettings.Capacity);
            _visualizerFactory.Create().Setup(storage, _storageSettings);
            if (_collectZone != null) _collectZone.AssignItemsProvider(storage);
            return storage;
        }
    }
}
