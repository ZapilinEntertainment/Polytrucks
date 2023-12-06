using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

	public enum Region : byte { UnknownRegion}
	public enum PointType : byte { UnknownPoint }
	public class PointOfInterest
	{
		public Region Region { get; protected set; }
		public PointType PointType { get; protected set; }
	}
}
