using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
	{
        [SerializeField] private int _requestExperienceRewardBase = 1;
        [SerializeField] private float _experienceMultiplier = 1f;
        [ SerializeField] private QuestTypeDefinedValues<int> _questExperienceRewards;

        public float ExperienceMultiplier => _experienceMultiplier;
        public int ExperienceRewardBase => _requestExperienceRewardBase;
        public int GetQuestExperienceReward(QuestType type) => _questExperienceRewards[type];
	}
}
