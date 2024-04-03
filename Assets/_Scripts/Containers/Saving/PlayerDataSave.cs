using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IPlayerDataSave
	{
		
		public float IntegrityPercent { get; set; }
		public TruckID PlayerTruckID { get; set; }
		public VirtualPoint RecoveryPoint { get; set; }
        public IntCompleteMask TutorialCompleteStatus { get; set; }

        public void UnlockTruck(TruckID id);
        public bool IsTruckUnlocked(TruckID id);

		public IReadOnlyList<VirtualCollectable> GetVehicleCargo();
	}
	public class PlayerDataSave : IPlayerDataSave
	{
		private List<VirtualCollectable> _storageContent = new();
		private HashSet<TruckID> _unlockedTrucksList= new() { GameConstants.DefaultTruck};
		public float IntegrityPercent { get; set; }
		public TruckID PlayerTruckID { get; set; }
		public VirtualPoint RecoveryPoint { get; set; }
        public IntCompleteMask TutorialCompleteStatus { get; set; }

        private PlayerDataSave() { }
		public PlayerDataSave(PlayerDataSavePreset preset)
		{
			if (preset.TutorialCompleted) TutorialCompleteStatus = TutorialAdviceIDExtension.GetFullMask();
			else TutorialCompleteStatus = TutorialAdviceIDExtension.GetEmptyMask();
			PlayerTruckID= preset.PlayerTruckID;
			RecoveryPoint = preset.RecoveryPoint;
            IntegrityPercent = preset.IntegrityPercent;
			var trucksList = preset.UnlockedTrucks;
			if (trucksList.Length > 0)
			{
				foreach (var truck in trucksList)
				{
					_unlockedTrucksList.Add(truck);
				}
			}
			_storageContent = preset.StorageContent;
		}
		public static PlayerDataSave Default
		{
			get
			{
				var save = new PlayerDataSave();
				save.TutorialCompleteStatus = TutorialAdviceIDExtension.GetEmptyMask();
				save.IntegrityPercent = 1f;
				save.PlayerTruckID = GameConstants.DefaultTruck;
				save.RecoveryPoint = new VirtualPoint(Vector3.zero, Quaternion.identity);
				return save;
			}
		}

		public void UnlockTruck(TruckID id) => _unlockedTrucksList.Add(id);
		public bool IsTruckUnlocked(TruckID id) => _unlockedTrucksList.Contains(id);

		public IReadOnlyList<VirtualCollectable> GetVehicleCargo() => _storageContent;
    }
   
}
