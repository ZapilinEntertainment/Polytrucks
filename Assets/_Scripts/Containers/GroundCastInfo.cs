using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public struct GroundCastInfo
	{
		public readonly float Resistance, AdditionalDepth;

		public GroundCastInfo(float resistance, float depth)
		{
			Resistance= resistance;
			AdditionalDepth= depth;
		}
	}
}
