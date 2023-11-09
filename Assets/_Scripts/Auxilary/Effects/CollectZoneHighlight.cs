using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class CollectZoneHighlight : TradeZoneHighlight
	{
		[SerializeField] private CollectZone _collectZone;

        protected override void CheckContract()
        {
            _activeContract = _player.FormCollectContract();
            _contractIsSuitable = _activeContract.IsValid && _collectZone.CanFulfillContract(_activeContract);
            _contractUpdateRequested = false;
            _effectCheckRequested = true;
        }
        protected override void SubscribeToZoneChanges()
        {
            _collectZone.OnItemAddedEvent += CheckContract;
        }
    }
}
