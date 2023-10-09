using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    [Serializable]
    public class StorageVisualSettings
    {
        public Transform _zeroPoint;
        public float Gap = 0.1f, ModelScale = 1f;
        public Vector3Int Extents = new Vector3Int(1, 1, 1);
		public int Width => Extents.x;
		public int Height => Extents.y;
		public int Length => Extents.z;
		public int ItemsInLayer => Width * Length;

        public int Capacity => Extents.x * Extents.y * Extents.z;
    }
    public class StorageVisualizer : ITickable, IDisposable
	{	
		private bool _isSetup = false, _isCompositionChanged = false;
		private StorageVisualSettings _visualSettings;
		private Storage _storage;
		private ObjectsManager _objectsManager;
		private CollectibleVisualRepresentation[] _collectibles;

		public StorageVisualizer(ObjectsManager manager, TickableManager tickableManager)
		{
			_objectsManager= manager;
			tickableManager.Add(this);
		}

		public void Setup(Storage storage, StorageVisualSettings settings)
		{
			_storage = storage;
			_visualSettings = settings;
			
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
			int length = _storage.Capacity, itemsInLayer = _visualSettings.ItemsInLayer, width = _visualSettings.Width;
			float gap = _visualSettings.Gap, scale = _visualSettings.ModelScale;
			const float crateSize = GameConstants.DEFAULT_COLLECTABLE_SIZE;
			float step = (crateSize * scale) + gap;
			Transform host = _visualSettings._zeroPoint;

			for (int i = 0; i < length; i++)
			{
				bool modelPresented = _collectibles[i] != null;
				var itemInfo = contents[i];
				if (itemInfo.CollectableType == CollectableType.Undefined)
				{
					if (modelPresented) ClearCell(i);
				}
				else
				{
					bool setNewModel = false;
					if (modelPresented)
					{
						if (!itemInfo.EqualsTo(_collectibles[i]))
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

                        model.parent = host;
                        model.localRotation = Quaternion.identity;

                        int layer = i / itemsInLayer, indexInLayer = i % itemsInLayer;
                        int xpos = indexInLayer % width, zpos = indexInLayer / width;
                        model.localPosition = new Vector3(
                            xpos * step,
                            layer * step,
                            zpos * step
                            );
                        model.localScale = scale * Vector3.one;

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
