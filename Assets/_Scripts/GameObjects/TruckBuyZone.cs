using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class TruckBuyZone : PlayerTrigger
	{
		[SerializeField] private TruckID _truckID;
		[SerializeField] private GameObject _object;
		[SerializeField] private Transform _tradeWindowPoint;		
		private int _labelViewID = -1;
		private SignalBus _signalBus;
		private TruckBuySystem _buySystem;

		[Inject]
		public void Inject(IAccountDataAgent accountData,  SignalBus signalBus, TruckBuySystem buySystem)
		{
			if (accountData.PlayerDataAgent.IsTruckUnlocked(_truckID)) HideZone();
			else
			{
				_signalBus = signalBus;
				_signalBus.Subscribe<TruckUnlockedSignal>(HideZone);
				_buySystem = buySystem;
			}
		}

        protected override void OnPlayerEnter(PlayerController player)
        {
            base.OnPlayerEnter(player);
            _buySystem.OnTruckDealStarted(_truckID, _tradeWindowPoint.position, Radius);
        }

        public void HideZone()
		{
			Destroy(_object);
		}
	}
}
