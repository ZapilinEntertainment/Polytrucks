using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IIntegrityConfiguration 
	{
		public int MaxHP { get; }
		public float HpDegradeSpeed { get; }
		public float HitIncomeDamageCf { get; }
		public float HitDamageLowLimit { get; }
	}
}
