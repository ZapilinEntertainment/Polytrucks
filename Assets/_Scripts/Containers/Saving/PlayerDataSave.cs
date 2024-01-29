using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IPlayerDataSave
	{
		public TruckID PlayerTruckID { get; }
		public VirtualPoint RecoveryPoint { get; }
	}
	public struct PlayerDataSave : IPlayerDataSave
	{
		public TruckID PlayerTruckID { get; private set; }
		public VirtualPoint RecoveryPoint { get; private set; }

		public PlayerDataSave(TruckID truckID, VirtualPoint recoveryPoint)
		{
			PlayerTruckID= truckID;
			RecoveryPoint = recoveryPoint;
		}

		public static PlayerDataSave Default => new PlayerDataSave(TruckID.TractorRosa, new VirtualPoint(Vector3.zero, Quaternion.identity));
	}
   
}
