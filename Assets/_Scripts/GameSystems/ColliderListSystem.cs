using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class ColliderListSystem
	{
		private class ColliderOwnersList<T> where T : IColliderOwner
		{
			private Dictionary<int, T> _list = new Dictionary<int, T>();
			public void AddOwner(T owner)
			{
				if (owner.HasMultipleColliders)
				{
					var ids = owner.GetIDs();
					foreach (var id in ids ) { _list.Add(id, owner); }
				}
				else
				{
					_list.Add(owner.GetID(), owner);
				}
			}
			public void RemoveOwner(T owner)
			{
				if (owner.HasMultipleColliders)
				{
                    var ids = owner.GetIDs();
                    foreach (var id in ids) { _list.Remove(id); }
                }
				else
				{
					_list.Remove(owner.GetID());
				}
			}
			public bool TryGetOwner(int id, out T owner)
			{
				return _list.TryGetValue(id, out owner);
			}
		}

		private ColliderOwnersList<ICollectable> _collectables = new ColliderOwnersList<ICollectable>();
		private ColliderOwnersList<ICollector> _collectors = new ColliderOwnersList<ICollector>();
		private ColliderOwnersList<ISeller> _sellers = new ColliderOwnersList<ISeller>();
		
		public void AddCollectable(ICollectable collectable) => _collectables.AddOwner(collectable);
		public void RemoveCollectable(ICollectable collectable) => _collectables.RemoveOwner(collectable);
		public bool TryGetCollectable(int id, out ICollectable collectable) => _collectables.TryGetOwner(id, out collectable);

		public void AddCollector(ICollector collector) => _collectors.AddOwner(collector);
		public void RemoveCollector(ICollector collector) => _collectors.RemoveOwner(collector);
		public bool TryGetCollector(int id, out ICollector collector) => _collectors.TryGetOwner(id, out collector);

		public void AddSeller(ISeller seller) => _sellers.AddOwner(seller);
		public void RemoveSeller(ISeller seller) => _sellers.RemoveOwner(seller);
		public bool TryGetSeller(int id, out ISeller seller) =>_sellers.TryGetOwner(id, out seller);
		
	}
}