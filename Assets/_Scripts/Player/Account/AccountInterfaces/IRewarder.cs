using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks.AccountData
{

    public enum RewardSource : byte { Undefined, Test }
    public enum RewardType : byte { Undefined, Money }
    public struct RewardInfoContainer
    {
        public int Value;
        public RewardType Type;
        public RewardSource Source;
    }
    public interface IRewarder
	{
		public void ApplyReward(RewardInfoContainer rewardInfo);
	}
}
