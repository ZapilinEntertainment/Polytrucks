using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISaveContainer
	{
		public void SavePlayerPosition(VirtualPoint point);
		public VirtualPoint LoadPlayerPosition();
	}
}
