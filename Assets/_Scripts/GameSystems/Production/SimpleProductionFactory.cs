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
        [SerializeField] private ConveyorBelt _inputBelt, _outputBelt;
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
            _sellZone.AssignStorage(_inputStorage);
            _collectZone.AssignStorage(_outputStorage);
            _productionModule.Setup(_recipe, _inputStorage, _outputStorage);
            _productionModule.TryStartProducing();
        }
    }
}
