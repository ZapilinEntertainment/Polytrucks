using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsObject", order = 1)]
    public sealed class GameSettings : ScriptableObject
	{
        [ SerializeField] private QuestTypeDefinedValues<int> _questExperienceRewards;

        public int GetQuestExperienceReward(QuestType type) => _questExperienceRewards[type];
	}
}
