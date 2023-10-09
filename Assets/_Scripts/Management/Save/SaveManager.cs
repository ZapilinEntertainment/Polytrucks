using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SaveManager
	{
		private ISaveContainer _saveContainer;

		public SaveManager()
		{
			_saveContainer = new LocalSave();
		}

		public void SavePlayerPoint(VirtualPoint point)
		{
			_saveContainer.SavePlayerPosition(point);
		}
		public VirtualPoint LoadPlayerPoint()
		{
			return _saveContainer.LoadPlayerPosition();
		}
	}
}
