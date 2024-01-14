using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks.AccountData {
	public interface IPlayerDataAgent
	{
        public int Money { get; }
        public Experience Experience { get; }
        public void OnPlayerSoldItem(SellOperationContainer sellOperation);
        public void SubscribeToMoneyChange(Action<int> action);
        public bool TrySpendMoney(int x);
    }
}
