using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class PlayerData : IInitializable
	{
		private SignalBus _signalBus;
		private Experience.Factory _experienceFactory;
        private Action<int> OnMoneyChangedEvent;
        public int Money { get; private set; }
		public Experience Experience { get; private set; }

		public PlayerData(SignalBus signalBus, InitializableManager init, Experience.Factory experienceFactory) {
			_signalBus= signalBus;		
			_experienceFactory= experienceFactory;
			init.Add(this);
		}
        public void Initialize()
        {
            Experience = _experienceFactory.Create();
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
		public bool TrySpendMoney(int x)
		{
			if (Money >= x)
			{
				Money -= x;
				return true;
			}
			else return false;
		}

        
    }
}
