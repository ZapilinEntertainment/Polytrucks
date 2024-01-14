using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	
	public class RewardService : IRewarder
	{
		private PlayerData _playerData;
		public RewardService(PlayerData playerData)
		{
			_playerData = playerData;
		}

		public void ApplyReward(RewardInfoContainer info)
		{
			switch(info.Type)
			{
				case RewardType.Money: _playerData.AddMoney(info.Value); break;
			}
		}
	}
}
