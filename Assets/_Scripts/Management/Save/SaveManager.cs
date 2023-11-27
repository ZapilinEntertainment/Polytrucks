using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SaveManager
	{
		public ISaveContainer SaveContainer { get; private set; }

		public SaveManager()
		{
			SaveContainer = new LocalSave();
		}

		public void SavePlayerPoint(VirtualPoint point)
		{
			SaveContainer.SavePlayerPosition(point);
		}
		public VirtualPoint LoadPlayerPoint()
		{
			return SaveContainer.LoadPlayerPosition();
		}
	}
}
