using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public class StorageVisualizer : ITickable, IDisposable
	{
		private VisualStorageSettingsBase _visualStorageSettings;
		private bool _isSetup = false, _isCompositionChanged = false;
		
		private Storage _storage;
		private ObjectsCreateService _objectsManager;
		private CollectibleVisualRepresentation[] _collectibles;

		public StorageVisualizer(ObjectsCreateService manager, TickableManager tickableManager)
		{
			_objectsManager= manager;
			tickableManager.Add(this);
		}

		public void Setup(Storage storage, VisualStorageSettingsBase settings)
		{
			_storage = storage;
            _visualStorageSettings = settings;
			
			_collectibles = new CollectibleVisualRepresentation[_storage.Capacity];
            CompareCompositionAndModels();
            _storage.OnStorageCompositionChangedEvent += OnCompositionChanged;
			_isSetup = true;
        }
		private void OnCompositionChanged()
		{
			_isCompositionChanged = true;
		}
		private void CompareCompositionAndModels()
		{
			var contents = _storage.GetContents();
			var storageConfiguration = _visualStorageSettings.GetStorageConfiguration();
			int contentsLength = contents?.Length ?? 0, length = _storage.Capacity, itemsInLayer = storageConfiguration.ItemsInLayer, width = storageConfiguration.Width;
			float gap = storageConfiguration.Gap, scale = storageConfiguration.ModelScale;
			const float crateSize = GameConstants.DEFAULT_COLLECTABLE_SIZE;
			float step = (crateSize * scale) + gap;

            for (int i = 0; i < length; i++)
			{
				bool modelPresented = _collectibles[i] != null;
				
				VirtualCollectable itemInfo = i < contentsLength ? contents[i] : VirtualCollectable.Empty;
				if (itemInfo.CollectableType == CollectableType.Undefined)
				{
					if (modelPresented) ClearCell(i);
				}
				else
				{
					bool setNewModel = false;
					if (modelPresented)
					{
						if (itemInfo != _collectibles[i])
						{
							ClearCell(i);
							setNewModel = true;
						}
					}
					else
					{
						setNewModel = true;
					}
					if (setNewModel)
					{
						Transform model;
                        var collectible = _objectsManager.GetCollectibleModel(itemInfo);
                        _collectibles[i] = new CollectibleVisualRepresentation(itemInfo.CollectableType, itemInfo.Rarity, collectible);

                        model = collectible.transform;

                        model.parent = _visualStorageSettings.ZeroPoint;
                        model.localRotation = Quaternion.identity;

                        int layer = i / itemsInLayer, indexInLayer = i % itemsInLayer;
                        int xpos = indexInLayer % width, zpos = indexInLayer / width;
                        model.localPosition = new Vector3(
                            (xpos + 0.5f) * step,
                            (layer + 0.5f) * step,
                            (zpos + 0.5f) * step
                            );
                        model.localScale = scale * Vector3.one;
						collectible.OnAllPropertiesSet();
                    }
				}
			}

			void ClearCell(int i)
			{
                _collectibles[i].Dispose();
                _collectibles[i] = null;
            }
		}

        public void Tick()
		{
			if (_isCompositionChanged & _isSetup)
			{
				CompareCompositionAndModels();
				_isCompositionChanged = false;
			}
		}
		public void Dispose()
		{
			if (_storage != null)
			{
				_storage.OnStorageCompositionChangedEvent -= OnCompositionChanged; _storage = null;
			}
		}

		public class Factory :PlaceholderFactory<StorageVisualizer> {
        }
    }
}
