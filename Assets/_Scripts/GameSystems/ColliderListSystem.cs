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
				if (owner.HaveMultipleColliders)
				{
					var ids = owner.GetColliderIDs();
					foreach (var id in ids ) { _list.Add(id, owner); }
				}
				else
				{
					_list.Add(owner.GetColliderID(), owner);
				}
			}
			public void OnOwnerChanged(T owner)
			{
                if (owner.HaveMultipleColliders)
                {
                    var ids = owner.GetColliderIDs();
                    foreach (var id in ids) { _list.TryAdd(id, owner); }
                }
                else
                {
                    _list.TryAdd(owner.GetColliderID(), owner);
                }
            }
			public void RemoveOwner(T owner)
			{
				if (owner.HaveMultipleColliders)
				{
                    var ids = owner.GetColliderIDs();
                    foreach (var id in ids) { _list.Remove(id); }
                }
				else
				{
					_list.Remove(owner.GetColliderID());
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
		private ColliderOwnersList<PlayerController> _playerColliders = new ColliderOwnersList<PlayerController>();
		private Dictionary<int, GroundInfoCollider> _groundColliders = new Dictionary<int, GroundInfoCollider>();
		
		public void AddCollectable(ICollectable collectable) => _collectables.AddOwner(collectable);
		public void RemoveCollectable(ICollectable collectable) => _collectables.RemoveOwner(collectable);
		public bool TryGetCollectable(int id, out ICollectable collectable) => _collectables.TryGetOwner(id, out collectable);

		public void AddCollector(ICollector collector) => _collectors.AddOwner(collector);
		public void RemoveCollector(ICollector collector) => _collectors.RemoveOwner(collector);
		public void OnCollectorChanged(ICollector collector) => _collectors.OnOwnerChanged(collector);
		public bool TryGetCollector(int id, out ICollector collector) => _collectors.TryGetOwner(id, out collector);

		public void AddSeller(ISeller seller) => _sellers.AddOwner(seller);
		public void RemoveSeller(ISeller seller) => _sellers.RemoveOwner(seller);
		public void OnSellerChanged(ISeller seller) => _sellers.OnOwnerChanged(seller);
		public bool TryGetSeller(int id, out ISeller seller) =>_sellers.TryGetOwner(id, out seller);

		public void AddPlayerColliders( PlayerController player) => _playerColliders.AddOwner(player);
		public void RemovePlayerCollider(PlayerController player) => _playerColliders.RemoveOwner(player);
		public bool TryDefineAsPlayer(int id, out PlayerController player) => _playerColliders.TryGetOwner(id, out player);

		public void AddGroundInfo(GroundInfoCollider groundCollider)
		{
			_groundColliders.Add(groundCollider.GetColliderID(), groundCollider);
		}
		public bool TryGetGroundInfoCollider(int id, out GroundInfoCollider collider)  
		{
			if (_groundColliders.TryGetValue(id, out collider))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public void RemoveGroundInfo(GroundInfoCollider groundCollider)
		{
			_groundColliders.Remove(groundCollider.GetColliderID());
		}
		
	}
}
