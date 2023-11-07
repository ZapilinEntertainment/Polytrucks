using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class SellModule : TradeModule,ISeller
	{
        protected ISellZone _sellZone;
		protected IVehicleController _vehicleController;
        public SellModule(TradeCollidersHandler collidersHandler, ColliderListSystem colliderListSystem, VehicleStorageController storageController, Vehicle vehicle) : base(collidersHandler, colliderListSystem, storageController)
        {
            Storage.OnItemAddedEvent += OnStorageCompositionChanged;
			_vehicleController = vehicle.VehicleController;
			vehicle.OnVehicleControllerChangedEvent += (IVehicleController controller) => _vehicleController = controller;

			colliderListSystem.AddSeller(this);
        }

        override public void Update()
        {
            if (_isInTradeZone)
			{
				if (_sellZone.IsReadyToReceive)
				{
					if (_storageCompositionChanged)
					{
						FormTradeContract();
					}
					if (_enoughGoodsForTrading)
					{
						var item = _preparedItemsList.Pop();
						if (Storage.TryExtractItem(item))
						{
							if (!TryReceiveItem(item)) Storage.ReturnItem(item);
						}
						_enoughGoodsForTrading = _preparedItemsList.Count != 0;
					}
                }
			}
        }
		virtual protected bool TryReceiveItem(VirtualCollectable item) => _sellZone.TrySellItem(this, item);


        public void OnEnterSellZone(ISellZone zone)
		{
			if (_isInTradeZone) i_OnExitSellZone();
			_sellZone = zone;
			_isInTradeZone = true;
			_activeContract = zone.FormTradeContract();
			FormTradeContract();
		}
		private void FormTradeContract()
		{
            if (_activeContract.IsValid && Storage.TryFormItemsList(_activeContract, out var list))
            {
                _preparedItemsList = new Stack<VirtualCollectable>(list);
				_enoughGoodsForTrading = _preparedItemsList.Count > 0;
            }
            else _enoughGoodsForTrading = false;
			_storageCompositionChanged = false;
        }
		public void OnExitSellZone(ISellZone zone)
		{
			if (zone == _sellZone)
			{
				i_OnExitSellZone();
			}
		}
		private void i_OnExitSellZone()
		{
            _sellZone = null;
			_isInTradeZone = false;
			_preparedItemsList?.Clear();
			_enoughGoodsForTrading = false;
        }
		
     
        public bool TryStartSell(TradeContract contract, out List<VirtualCollectable> list) => Storage.TryFormItemsList(contract, out list);
        public void OnItemSold(SellOperationContainer sellInfo) => _vehicleController?.OnItemSold(sellInfo);

		protected override void OnColliderListChanged() => _colliderListSystem.OnSellerChanged(this);
    }
}
