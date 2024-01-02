using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class RecoveryPoint : MonoBehaviour
	{
		[SerializeField] protected Transform _point;

		public VirtualPoint GetPoint() => new VirtualPoint(_point);
	}
}
