using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public class ProductionModule : ITickable
    {
        private bool _isProducing = false, _needToRecalculateResources = false;
        private float _progress = 0f;
        private int _resourcesCount = 0, _delayedOutput = 0;
        private Recipe _recipe;
        private IItemProvider _input;
        private IItemReceiver _output;

        [Inject]
        public void Inject(TickableManager tickableManager)
        {
            tickableManager.Add(this);
        }

        public void Setup(Recipe recipe, IItemProvider input, IItemReceiver output)
        {
            _recipe = recipe;
            _input = input;
            _output = output;

            _input.SubscribeToProvisionListChange(OnItemAddedToInput);
            _output.SubscribeToItemRemoveEvent(OnOutputItemSold);
        }
        private void OnItemAddedToInput()
        {
            _needToRecalculateResources = true;
            if (!_isProducing) TryStartProducing();
        }
        private void OnOutputItemSold()
        {
            if (_delayedOutput > 0)
            {
                if (_delayedOutput == 1)
                {
                    if (_output.TryAddItem(_recipe.ResultItem)) _delayedOutput = 0;
                }
                else
                {
                    var resultItem = _recipe.ResultItem;
                    int count = _delayedOutput;
                    for (int i = 0; i < count; i++)
                    {
                        if (_output.TryAddItem(resultItem)) _delayedOutput--;
                        else break;
                    }
                }
                if (_delayedOutput == 0) TryStartProducing();
            }
        }

        public void TryStartProducing()
        {
            if (!_isProducing && _input.AvailableItemsCount >= _recipe.InputValue && _delayedOutput == 0)
            {
                ProductionCheck();
            }
        }
        protected void ProductionCheck()
        {
            _isProducing = _input.TryExtractItems(new VirtualCollectable(_recipe.Input, _recipe.InputRarity), _recipe.InputValue);
        }     
        
        public void Tick()
        {
            if (_isProducing)
            {                
                _progress += Time.deltaTime / _recipe.ProductionTime;
                if (_progress > 1f)
                {
                    if (_needToRecalculateResources)
                    {
                        UpdateResourcesCount();
                    }
                    else
                    {
                        _resourcesCount -= _recipe.InputValue;
                    }

                    if (_resourcesCount < _recipe.InputValue)
                    {
                        StopProduction();
                    }
                    else _progress--;


                    if (!_output.TryAddItem(_recipe.ResultItem)) {
                        _delayedOutput++;
                        StopProduction();
                    }
                    else
                    {
                        ProductionCheck();
                    }
                }
            }
            else
            {
                if (_needToRecalculateResources) UpdateResourcesCount();                
            }
        }
        private void UpdateResourcesCount()
        {
            _resourcesCount = _input.CalculateItemsCount(_recipe.Input, _recipe.InputRarity);
            _needToRecalculateResources = false;
            TryStartProducing();
        }
        
        private void StopProduction()
        {
            _progress = 0f;
            _isProducing = false;
        }

        public class Factory : PlaceholderFactory<ProductionModule>
        {
        }
    }
}
