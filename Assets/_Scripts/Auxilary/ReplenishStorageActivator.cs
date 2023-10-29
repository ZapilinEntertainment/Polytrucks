using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class ReplenishStorageActivator : MonoBehaviour
	{
		[SerializeField] private SellZoneBase _sellZoneTarget;
		[SerializeField] private ReplenishableStorage _replenishableStorage;
        private bool _itemSold = false;

        private void Start()
        {
            _sellZoneTarget.OnAnyItemSoldEvent += OnItemSold;
        }
        private void OnItemSold()
        {
            if (!_itemSold)
            {
                _itemSold = true;
                _replenishableStorage.SetActivity(true);
                Destroy(this);
            }
        }
    }
}
