using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IPlayerDataSave
	{
		public float IntegrityPercent { get; }
		public TruckID PlayerTruckID { get; }
		public VirtualPoint RecoveryPoint { get; }
	}
	public struct PlayerDataSave : IPlayerDataSave
	{
		public float IntegrityPercent { get; private set; }
		public TruckID PlayerTruckID { get; private set; }
		public VirtualPoint RecoveryPoint { get; private set; }

		public PlayerDataSave(float integrityPercent, TruckID truckID, VirtualPoint recoveryPoint)
		{
			PlayerTruckID= truckID;
			RecoveryPoint = recoveryPoint;
            IntegrityPercent = integrityPercent;
		}

		public static PlayerDataSave Default => new PlayerDataSave(1f, TruckID.TractorRosa, new VirtualPoint(Vector3.zero, Quaternion.identity));
	}
   
}
