using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class PlayerData 
	{
		private SignalBus _signalBus;
        private Action<int> OnMoneyChangedEvent;
        public int Money { get; private set; }

		public PlayerData(SignalBus signalBus) {
			_signalBus= signalBus;
		}

		private void AddMoney(int x)
		{
			Money += x;
			OnMoneyChangedEvent?.Invoke(Money);
		}
		public void SubscribeToMoneyChange(Action<int> del) => OnMoneyChangedEvent += del;
		

		public void OnPlayerSoldItem(SellOperationContainer sellOperation)
		{
			AddMoney(sellOperation.MoneyCount);
			_signalBus.Fire(new PlayerItemSellSignal(sellOperation));
		}
	}
}
