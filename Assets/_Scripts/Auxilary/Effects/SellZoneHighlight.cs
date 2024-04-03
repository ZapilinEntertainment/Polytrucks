using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SellZoneHighlight : TradeZoneHighlight
	{
		[SerializeField] private SellZoneBase _sellZone;

        protected override void CheckContract()
        {
            _activeContract = _sellZone.FormTradeContract();
            _contractIsSuitable = _activeContract.IsValid && _player.CanFulfillContract(_activeContract);
            _contractUpdateRequested = false;
            _effectCheckRequested = true;
        }
        protected override void SubscribeToZoneChanges() { }
    }
}
