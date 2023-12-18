using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public class Experience
	{
        public int Points { get; private set; }
        public int Level { get; private set; }
        public int PointsToNextLevel { get; private set; }
        public float ProgressPercent => Points / (float)PointsToNextLevel;

        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        public Action OnExperienceCountChangedEvent;

        public Experience(PlayerData playerData, SignalBus signalBus, GameSettings gameSettings) {
            Points = 0;
            Level= 0;
            PointsToNextLevel = GetExperienceLimit(Level);

            _signalBus= signalBus;
            _signalBus.Subscribe<QuestCompletedSignal>(OnQuestCompleted);

            _gameSettings= gameSettings;
        }

        private void OnQuestCompleted(QuestCompletedSignal signal) => AddExperiencePoints(signal.Quest.GetExperienceReward(_gameSettings));

        private void AddExperiencePoints(int x)
        {
            Points += x;
            if (Points >= PointsToNextLevel)
            {
                LevelUp();
            }
            OnExperienceCountChangedEvent?.Invoke();
        }
        private void LevelUp()
        {
            while (Points > PointsToNextLevel)
            {
                Level++;
                Points -= PointsToNextLevel;
                PointsToNextLevel = GetExperienceLimit(Level);
                _signalBus.Fire(new PlayerLevelUpSignal(Level));
            }
        }
        private int GetExperienceLimit(int level)
        {
            return level * 2 + 2;
        }

        public class Factory : PlaceholderFactory<Experience>
        {
        }
    }
}
