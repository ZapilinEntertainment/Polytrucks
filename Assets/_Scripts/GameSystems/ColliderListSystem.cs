using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class ColliderListSystem
	{

		private Dictionary<int, ICollectable> _collectables = new Dictionary<int, ICollectable>();
		private Dictionary<int, ICollector> _collectors = new Dictionary<int, ICollector>();

		public void AddCollectable(ICollectable collectable)
		{
			_collectables.Add(collectable.GetID(), collectable);
		}
		public void RemoveCollectable(int id) => _collectables.Remove(id);

		public void AddCollector(ICollector collector) {
			var ids = collector.GetIDs();
			foreach (var id in ids)
			{
				_collectors.Add(id, collector);
			}
		}
		public void RemoveCollector(ICollector collector) {
			var ids = collector.GetIDs();
			foreach (var id in ids)
			{
				_collectors.Remove(id);
			}
		}
		public bool TryGetCollector(int id, out ICollector collector) => _collectors.TryGetValue(id, out collector);
	}
}
