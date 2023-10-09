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
