using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	public sealed class PlayerData : IPlayerDataAgent
	{
		private SignalBus _signalBus;
        private Action<int> OnMoneyChangedEvent;
        public int Money { get; private set; }
		public Experience Experience { get; private set; }

		public PlayerData(SignalBus signalBus, GameSettings gameSettings) {
			_signalBus= signalBus;		
			Experience= new Experience(signalBus, gameSettings);
		}

        public void AddMoney(int x)
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
		public bool TrySpendMoney(int x)
		{
			if (Money >= x)
			{
				Money -= x;
				OnMoneyChangedEvent?.Invoke(Money);
				return true;
			}
			else return false;
		}

        
    }
}
