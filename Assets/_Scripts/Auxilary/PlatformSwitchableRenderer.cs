using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

	public abstract class PlatformSwitchableRenderer : MonoBehaviour
	{
		public abstract void SetState(PlatformState state);
	}
}
