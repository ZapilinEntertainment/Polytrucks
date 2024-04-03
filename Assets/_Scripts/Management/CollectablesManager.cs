using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class CollectablesManager 
	{
		private HashSet<ICollectable> _collectables = new();
		public ICollection<ICollectable> Collectables => _collectables;

		public void AddCollectable(ICollectable collectable)
		{
			_collectables.Add(collectable);
		}
		public void RemoveCollectable(ICollectable collectable)
		{
			_collectables.Remove(collectable);
		}
	}
}
