using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	public sealed class PlayerData : IPlayerDataAgent
	{
		public TruckID ActiveTruckID { get; private set; }
		private VirtualPoint _recoveryPoint;
		private IPlayerDataSave _dataSave;
		private SignalBus _signalBus;
        private Action<int> OnMoneyChangedEvent;
        public int Money { get; private set; }
		public Experience Experience { get; private set; }

		public PlayerData(SignalBus signalBus, GameSettings gameSettings, IPlayerDataSave save) {
			_signalBus= signalBus;		
			_dataSave= save;
			Experience= new Experience(signalBus, gameSettings);

			ActiveTruckID = _dataSave.PlayerTruckID;
			_recoveryPoint= _dataSave.RecoveryPoint;
		}

        public void AddMoney(int x)
		{
			Money += x;
			OnMoneyChangedEvent?.Invoke(Money);
		}
		public void UnlockTruck(TruckID id)
		{
			_dataSave.UnlockTruck(id);
			_signalBus.Fire(new TruckUnlockedSignal(id));
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
		public bool IsTruckUnlocked(TruckID id) => _dataSave.IsTruckUnlocked(id);
        public bool TrySwitchTruck(TruckID id, out TruckSwitchReport msg)
		{
			if (IsTruckUnlocked(id))
			{
				msg = TruckSwitchReport.SwitchSucceed;
				ActiveTruckID = id;
				return true;
			}
			else
			{
				msg = TruckSwitchReport.TruckLockedError; 
				return false;
			}
		}
		public VirtualPoint GetRecoveryPoint()
		{
			return _recoveryPoint;
		}
    }
}
