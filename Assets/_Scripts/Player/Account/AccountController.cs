using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	public class AccountController : IAccountDataAgent
	{
		protected PlayerData _playerData;
		protected RewardService _rewardService;

		public IRewarder RewardAgent => _rewardService;
		public IPlayerDataAgent PlayerDataAgent => _playerData;

		public AccountController(SignalBus signalBus, GameSettings gameSettings, IPlayerDataSave playerDataSave)
		{
			_playerData = new PlayerData(signalBus, gameSettings, playerDataSave);
			_rewardService = new RewardService(_playerData);
		}
	}
}
