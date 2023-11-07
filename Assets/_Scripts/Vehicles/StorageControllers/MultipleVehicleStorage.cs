using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class MultipleVehicleStorage : VehicleStorageController, IStorage
    {
        [SerializeField] private StorageVisualSettings[] _storageSettings;
        private bool _storagesCompositionChanged = false;
        private int _count = 0;
        private StorageVisualizer.Factory _visualizerFactory;
        private List<Storage> _storages = new List<Storage>();
        public override IStorage Storage => this;
        public override Storage MainStorage => _storages[0];

        public int ItemsCount { get; private set; }
        public int FreeSlotsCount { get; private set; }
        public int Capacity { get; private set; }

        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }

        public int AvailableItemsCount => ItemsCount;
        public bool IsReadyToReceive => ItemsCount < Capacity;


        [Inject]
        public void Inject(StorageVisualizer.Factory factory)
        {            
            _visualizerFactory = factory;

            _count= _storageSettings.Length;
            _storages = new List<Storage>(_count);
            for (int i = 0; i < _count; i++)
            {
                var storage = new Storage(_storageSettings[i].Capacity);
                storage.OnStorageCompositionChangedEvent += OnStorageCompositionChanged;
                storage.OnStorageCompositionChangedEvent += OnStorageCompositionChanged;
                storage.OnItemAddedEvent += OnItemAddedEvent;
                storage.OnItemRemovedEvent += OnItemRemovedEvent;
                _storages.Add(storage);
            }
            UpdateValues();
        }
        private void OnStorageCompositionChanged()
        {
            _storagesCompositionChanged = true;
        }
        public void UpdateValues()
        {
            ItemsCount = 0;
            FreeSlotsCount = 0;
            Capacity = 0;
            foreach (var storage in _storages)
            {
                ItemsCount += storage.ItemsCount;
                FreeSlotsCount += storage.FreeSlotsCount;
                Capacity += storage.Capacity;
            }

            _storagesCompositionChanged = false;
        }
  

        public void Setup(StorageVisualSettings[] newSettings)
        {
            _storageSettings = newSettings;
        }
        private void Start()
        {
            for (int i = 0; i < _count; i++)
            {
                var visualizer = _visualizerFactory.Create();
                visualizer.Setup(_storages[i], _storageSettings[i]);
            }
        }
        private void Update()
        {
            if (_storagesCompositionChanged)
            {
                UpdateValues();
                OnStorageCompositionChangedEvent?.Invoke();
            }
        }

        public void AddStorage(Storage storage)
        {
            _storages.Add(storage);
            _storagesCompositionChanged = true;
        }
        public void AddStorage(StorageVisualSettings settings)
        {
            var storage = new Storage(settings.Capacity);
            var visualizer = _visualizerFactory.Create();
            visualizer.Setup(storage, settings);
            AddStorage(storage);
        }
        public void RemoveStorage(Storage storage)
        {
            _storages.Remove(storage);
            _storagesCompositionChanged = true;
        }

        #region istorage
        public void ReturnItem(VirtualCollectable item) => TryAddItem(item);
        public bool TryAddItem(VirtualCollectable item)
        {
            foreach (var storage in _storages)
            {
                if (storage.IsFull) continue;
                else
                {
                    if (storage.TryAddItem(item)) return true;
                }
            }
            return false;
        }
        public int AddItems(VirtualCollectable item, int count)
        {
            foreach (var storage in _storages)
            {
                if (storage.IsFull) continue;
                else
                {
                    count = storage.AddItems(item, count);
                    if (count == 0) return count;
                }
            }
            return count;
        }
        public void AddItems(IList<VirtualCollectable> items, out BitArray result)
        {
            int count = items.Count;
            result = new BitArray(count, false);

            var unsolvedList = new List<VirtualCollectable>(items); // items that still not add to any storage
            var unsolvedListIndices = new List<int>(count); // save indices of total - result - mask
            for (int i = 0; i < count; i++) unsolvedListIndices[i] = i;
            int unsolvedItemsCount = count;

            foreach (var storage in _storages)
            {
                if (storage.IsFull) continue;
                else
                {
                    storage.AddItems(unsolvedList, out var solvingMask);

                    var newUnsolvedList = new List<VirtualCollectable>();
                    var newUnsolvedIndices = new List<int>();
                    for (int i = 0; i < unsolvedItemsCount; i++)
                    {
                        bool itemResult = solvingMask[i];
                        int resultIndex = unsolvedListIndices[i];
                        result[resultIndex] = itemResult; // write to final result
                        if (!itemResult)
                        {
                            newUnsolvedList.Add(unsolvedList[i]);
                            newUnsolvedIndices.Add(resultIndex);
                        }
                    }

                    unsolvedItemsCount = newUnsolvedList.Count; // check if any items left unsolved
                    if (unsolvedItemsCount == 0) return;
                    else
                    {
                        unsolvedList = newUnsolvedList;
                        unsolvedListIndices = newUnsolvedIndices;
                    }
                }
            }
        }

        public int CalculateItemsCount(CollectableType type, Rarity rarity)
        {
            int count = 0;
            foreach (var storage in _storages)
            {
                count += storage.CalculateItemsCount(type, rarity);
            }
            return count;
        }        

     
        public bool TryFormItemsList(TradeContract contract, out List<VirtualCollectable> list)
        {
            list = new List<VirtualCollectable>();
            foreach (var storage in _storages)
            {
                if (storage.IsEmpty) continue;
                else
                {
                    if (storage.TryFormItemsList(contract, out var storageList)) list.AddRange(storageList);
                }
            }
            return list.Count > 0;
        }

        public bool TryExtractItem(VirtualCollectable item)
        {
            foreach (var storage in _storages)
            {
                if (storage.IsEmpty) continue;
                else
                {
                    if (storage.TryExtractItem(item)) return true;
                }
            }
            return false;
        }
        public bool TryExtractItems(VirtualCollectable item, int count)
        {
            if (count == 1) return TryExtractItem(item);
            else
            {
                List<(Storage storage, List<int> reserved)> foundItems = new();
                int countLeft = count;

                foreach (var storage in _storages)
                {
                    if (storage.IsEmpty) continue;
                    else
                    {
                        if (storage.TryReserveItems(item, countLeft, out var list))
                        {
                            foundItems.Add((storage, list));
                            countLeft -= list.Count;
                            if (countLeft == 0) break;
                        }
                    }
                }
                if (countLeft == 0)
                {
                    foreach (var info in foundItems)
                    {
                        info.storage.RemoveReservedItems(info.reserved);
                    }
                    return true;
                }
                else return false;
            }
        }
        public bool TryExtractItems(TradeContract contract, out List<VirtualCollectable> list)
        {
            list = new List<VirtualCollectable>();
            foreach (var storage in _storages)
            {
                if (storage.IsEmpty) continue;
                else
                {
                    if (storage.TryExtractItems(contract, out var storageList)) list.AddRange(storageList); 
                }
            }
            return list.Count > 0;
        }

        #region events
        public void SubscribeToItemAddEvent(Action action)
        {
            OnItemAddedEvent += action;
        }
        public void SubscribeToItemRemoveEvent(Action action)
        {
            OnItemRemovedEvent += action;
        }
        public void SubscribeToProvisionListChange(Action action)
        {
            OnStorageCompositionChangedEvent += action;
        }
        public void UnsubscribeFromItemAddEvent(Action action)
        {
            OnItemAddedEvent-= action;
        }
        public void UnsubscribeFromItemRemoveEvent(Action action)
        {
            OnItemRemovedEvent-= action;
        }
        public void UnsubscribeFromProvisionListChange(Action action)
        {
            OnStorageCompositionChangedEvent-= action;
        }
        #endregion
        #endregion
    }
}
