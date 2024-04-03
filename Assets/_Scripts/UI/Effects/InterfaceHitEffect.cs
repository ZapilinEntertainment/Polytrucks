using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class InterfaceHitEffect : MonoBehaviour
	{
		public abstract void Hit();
		public abstract void StopEffect();
	}
}
