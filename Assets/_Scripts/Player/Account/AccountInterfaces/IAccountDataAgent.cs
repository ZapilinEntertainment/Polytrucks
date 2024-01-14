using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZE.Polytrucks.AccountData;

namespace ZE.Polytrucks {
	public interface IAccountDataAgent
	{
		public IRewarder RewardAgent { get; }
		public IPlayerDataAgent PlayerDataAgent { get; }
	}
}
