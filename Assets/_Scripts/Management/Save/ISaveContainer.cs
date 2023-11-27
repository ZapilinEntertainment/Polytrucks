using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISaveContainer
	{
		public void SavePlayerPosition(VirtualPoint point);
		public VirtualPoint LoadPlayerPosition();

		public LocalizationLanguage LoadLocale();
		public void SaveLocale(LocalizationLanguage locale);
	}
}
