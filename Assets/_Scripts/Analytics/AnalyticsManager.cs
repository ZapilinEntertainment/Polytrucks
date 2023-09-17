using UnityEngine;
using Unity;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZE.Polytrucks
{   
    public sealed class AnalyticsManager : MonoBehaviour
    {
        [SerializeField] private bool _debugMode = false;
        [SerializeField] private MonoBehaviour[] _handlers;
        public Action<IAnalyticsEvent> OnAnalyticsEvent;
        public static AnalyticsManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            if (_debugMode) OnAnalyticsEvent += LogEvent;
            else
            {
                foreach (var handler in _handlers)
                {
                    (handler as IAnalyticSystemHandler).Setup(this);
                }
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (Saves.IsFirstStart()) OnAnalyticsEvent?.Invoke(new BaseAnalyticsEvent(AnalyticsEventType.FirstLaunch));
            OnAnalyticsEvent?.Invoke(new BaseAnalyticsEvent(AnalyticsEventType.GameStarted));
        }


        public static void OnLevelStarted(int x) => SendLevelEvent(AnalyticsEventType.LevelStarted, x);
        public static void OnLevelStarted(Biome biome, int x) => SendLevelEvent(AnalyticsEventType.LevelStarted, biome, x);
        public static void OnMissionStarted(int x)
        {
            if (Instance != null) Instance.OnAnalyticsEvent?.Invoke(new IntAnalyticsEvent(AnalyticsEventType.MissionStarted, x));
        }

        public static void OnLevelCompleted(int x) => SendLevelEvent(AnalyticsEventType.LevelCompleted, x);
        public static void OnLevelCompleted(Biome biome, int x) => SendLevelEvent(AnalyticsEventType.LevelCompleted, biome, x);
        public static void OnMissionCompleted(int x)
        {
            if (Instance != null) Instance.OnAnalyticsEvent?.Invoke(new IntAnalyticsEvent(AnalyticsEventType.MissionCompleted, x));
        }

        public static void OnLevelFailed(int x) => SendLevelEvent(AnalyticsEventType.LevelFailed, x);
        public static void OnLevelFailed(Biome biome, int x) => SendLevelEvent(AnalyticsEventType.LevelFailed, biome, x);


        private static void SendLevelEvent(AnalyticsEventType eventType, int levelIndex)
        {
           if (Instance != null) Instance.OnAnalyticsEvent?.Invoke(new IntAnalyticsEvent(eventType, levelIndex));
        }
        private static void SendLevelEvent(AnalyticsEventType eventType, Biome biome, int levelIndex)
        {
            if (Instance != null) Instance.OnAnalyticsEvent?.Invoke(new EnumAnalyticsEvent(eventType, (int)biome, levelIndex));
        }

        private void LogEvent(IAnalyticsEvent i_event)
        {
            string s = i_event.EventType.GetEventKey() + ": ";
            AnalyticsEventValues valuesMask = i_event.ValuesMask;
            if (valuesMask.HasFlag(AnalyticsEventValues.IntValue)) s +=  " int: " + i_event.IntValue.ToString();
            if (valuesMask.HasFlag(AnalyticsEventValues.EnumValue)) s += " enum: " + i_event.EnumValue.ToString();
            if (valuesMask.HasFlag(AnalyticsEventValues.StringValue)) s += " string: " + i_event.StringValue;
            print(s);
        }
    }
}
