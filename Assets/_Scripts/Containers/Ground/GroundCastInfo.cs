using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public struct GroundCastInfo
	{
		public readonly float Harshness, Resistance, AdditionalDepth;

		public GroundCastInfo(float harshness, float resistance, float additionalDepth)
		{
			Harshness = harshness;
			Resistance= resistance;
			AdditionalDepth= additionalDepth;
		}
	}
}
