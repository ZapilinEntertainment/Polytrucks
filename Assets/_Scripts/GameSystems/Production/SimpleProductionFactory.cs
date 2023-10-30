using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class SimpleProductionFactory : MonoBehaviour
	{
		[SerializeField] protected SingleItemSellZone _sellZone;
        [SerializeField] protected CollectZone _collectZone;
		[SerializeField] private StorageVisualSettings _inputStorageSettings, _outputStorageSettings;
        [SerializeField] private StorageReceiver _inputReceiver, _outputReceiver;
        [SerializeField] protected Recipe _recipe;
		protected Storage _outputStorage, _inputStorage;
        protected ProductionModule _productionModule;

        [Inject]
        public void Inject(Storage.Factory storageFactory, ProductionModule.Factory productionFactory)
        {
            _inputStorage = storageFactory.Create(_inputStorageSettings);
            _outputStorage = storageFactory.Create(_outputStorageSettings);
            _productionModule= productionFactory.Create();           
        }

        private void Start()
        {
            if (_inputReceiver != null)
            {
                _sellZone.AssignReceiver(_inputReceiver);
                _inputReceiver.AssignStorage(_inputStorage);
            }
            else
            {
                _sellZone.AssignReceiver(_inputStorage);
            }

            bool usingOutputReceiver = _outputReceiver != null;
            if (usingOutputReceiver)
            {
                _outputReceiver.AssignStorage(_outputStorage);
            }

            _collectZone.AssignItemsProvider(_outputStorage);
            _productionModule.Setup(_recipe, _inputStorage, usingOutputReceiver ? _outputReceiver : _outputStorage);
            _productionModule.TryStartProducing();
        }
    }
}
