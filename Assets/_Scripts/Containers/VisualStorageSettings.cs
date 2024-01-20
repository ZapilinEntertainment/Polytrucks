using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
	[System.Serializable]
	public class VisualStorageSettings : VisualStorageSettingsBase
	{
		[field: SerializeField] public StorageConfiguration StorageConfiguration { get; private set; }
		public override IStorageConfiguration GetStorageConfiguration() => StorageConfiguration;

        public int Capacity => StorageConfiguration.Capacity;

		public VisualStorageSettings(Transform zeroPoint, StorageConfiguration config)
		{
			ZeroPoint = zeroPoint;
			StorageConfiguration = config;
		}
	}

	public abstract class VisualStorageSettingsBase
	{
		[field:SerializeField] public Transform ZeroPoint { get; protected set; }
		abstract public IStorageConfiguration GetStorageConfiguration();
	}
	public class VirtualVisualStorageSettings : VisualStorageSettingsBase
	{
		private readonly IStorageConfiguration _storageConfig;
		public override IStorageConfiguration GetStorageConfiguration() => _storageConfig;

		public VirtualVisualStorageSettings(IStorageConfiguration storageConfig, Transform zeroPoint)
        {
            _storageConfig = storageConfig;
			ZeroPoint= zeroPoint;
        }
    }
}
