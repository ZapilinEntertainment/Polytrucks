using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISellZone
	{
        public bool TrySellItem(VirtualCollectable item);

    }
}
