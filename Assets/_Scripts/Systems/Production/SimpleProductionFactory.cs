using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class SimpleProductionFactory : MonoBehaviour
	{
		[SerializeField] protected SingleItemSellZone _sellZone;
        [SerializeField] protected CollectZone _collectZone;
		[SerializeField] private VisualStorageSettings _inputStorageSettings, _outputStorageSettings;
        [SerializeField] private StorageReceiver _inputReceiver, _outputReceiver;
        [SerializeField] protected Recipe _recipe;
		protected Storage _outputStorage, _inputStorage;
        protected ProductionModule _productionModule;

        private StorageVisualizer.Factory _visualizerFactory;

        [Inject]
        public void Inject(StorageVisualizer.Factory visFactory, ProductionModule.Factory productionFactory)
        {
            _visualizerFactory = visFactory;            
            _productionModule= productionFactory.Create();           
        }
        private void Awake()
        {
            _inputStorage = new Storage(_inputStorageSettings.Capacity);
            _outputStorage = new Storage(_outputStorageSettings.Capacity);
        }

        private void Start()
        {
            if (_inputStorageSettings.ZeroPoint != null) _visualizerFactory.Create().Setup(_inputStorage, _inputStorageSettings);
            if (_outputStorageSettings.ZeroPoint != null) _visualizerFactory.Create().Setup(_outputStorage, _outputStorageSettings);

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
