using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/HangarTrucksList")]
    public sealed class HangarTrucksList : ScriptableObject
	{
		[Serializable]
		public class TruckInfo
		{
			[field: SerializeField] public TruckID TruckID { get; private set; }
			[field: SerializeField] public Truck TruckPrefab { get; private set; }
			[field: SerializeField] public TruckConfig TruckConfig { get; private set; }
			[field: SerializeField] public WheelConfiguration WheelsConfig { get; private set; }
			[field: SerializeField] public Sprite Icon { get; private set; }
		}
		[Serializable]
		public class TrailerInfo
		{
			[field: SerializeField] public TrailerID TrailerID { get; private set; }
			[field: SerializeField] public Trailer TrailerPrefab { get; private set; }
			[field: SerializeField] public WheelConfiguration WheelConfiguration { get; private set; }
			[field: SerializeField] public StorageConfiguration StorageConfiguration { get; private set; }
		}
		
		[SerializeField] private TruckInfo[] _trucksInfo;
		[SerializeField] private TrailerInfo[] _trailersInfo;
        [field: SerializeField] public GameObject LockedTruckPlaceholder;

        public bool TryGetTruckInfo(TruckID id, out TruckInfo info)
        {
            info = GetTruckInfo(id);
            return info != null;
        }
        public int DefineTruckIndex(TruckID id, out TruckConfig config)
		{
			for (int i = 0; i < _trucksInfo.Length; i++)
			{
				if (_trucksInfo[i].TruckID == id)
				{
					config = _trucksInfo[i].TruckConfig;
					return i;
				}
			}
			config = null;
			return -1;
		}
		public TruckInfo GetTruckInfo(int index)
		{
            for (int i = 0; i < _trucksInfo.Length; i++)
            {
                if (index == i) return _trucksInfo[i];
            }
            return null;
        }
        public TruckInfo GetTruckInfo(TruckID id)
        {
            foreach (var info in _trucksInfo)
			{
				if (info.TruckID == id) return info;
			}
            return null;
        }
        public IReadOnlyList<TruckInfo> GetTrucksInfo() => _trucksInfo;
        


		public TrailerInfo GetTrailerInfo(TrailerID id)
		{
			foreach (var trailerInfo in _trailersInfo)
			{
				if (trailerInfo.TrailerID == id) return trailerInfo;
			}
			return null;
		}
		public bool TryGetTrailerInfo(TrailerID id, out TrailerInfo trailerInfo)
		{
			trailerInfo = GetTrailerInfo(id);
			return trailerInfo != null;
		}
    }
}
