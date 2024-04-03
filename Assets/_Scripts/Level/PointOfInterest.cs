using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

	public enum Region : byte { UnknownRegion}
	public enum PointType : byte { UnknownPoint }

	[System.Serializable]
	public class PointOfInterest
	{
		[field:SerializeField]public Region Region { get; protected set; }
        [field: SerializeField] public PointType PointType { get; protected set; }
	}
}
