using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    public enum AnalyticsEventType : byte { Undefined = 0, FirstLaunch, GameStarted, LevelStarted, LevelCompleted, LevelFailed,MissionStarted, MissionCompleted, ItemUnlocked, ItemUpgraded }
    public static class AnalyticsEventTypeExtension
    {
        public static string GetEventKey(this AnalyticsEventType eventType) => eventType.ToString();
    }
    public interface IAnalyticSystemHandler
    {
        public void Setup(AnalyticsManager manager);
    }

    [Flags]
    public enum AnalyticsEventValues : byte { NoValues = 0, IntValue = 1, EnumValue = 2, StringValue = 4 }
    public interface IAnalyticsEvent
    {
        public AnalyticsEventType EventType { get; }
        public AnalyticsEventValues ValuesMask { get; }
        public int IntValue { get; }
        public int EnumValue { get; }
        public string StringValue { get; }        
    }

    #region eventContainers
    public struct BaseAnalyticsEvent : IAnalyticsEvent
    {
        public readonly AnalyticsEventType EventType { get; }
        public int IntValue => 0;
        public int EnumValue => 0;
        public string StringValue => string.Empty;
        public AnalyticsEventValues ValuesMask => AnalyticsEventValues.NoValues;

        public BaseAnalyticsEvent(AnalyticsEventType eventType) { this.EventType = eventType; }
    }
    public struct IntAnalyticsEvent : IAnalyticsEvent
    {
        public readonly AnalyticsEventType EventType { get; }
        public readonly int IntValue { get; }
        public int EnumValue => 0;
        public string StringValue => string.Empty;

        public IntAnalyticsEvent(AnalyticsEventType eventType, int i_val)
        {
            this.EventType = eventType;
            this.IntValue = i_val;
        }
        public AnalyticsEventValues ValuesMask => AnalyticsEventValues.IntValue;
    }
    public struct EnumAnalyticsEvent : IAnalyticsEvent
    {
        public readonly AnalyticsEventType EventType { get; }
        public readonly int IntValue { get; }
        public readonly int EnumValue { get; }
        public string StringValue => string.Empty;
        public AnalyticsEventValues ValuesMask => AnalyticsEventValues.IntValue | AnalyticsEventValues.EnumValue;

        public EnumAnalyticsEvent(AnalyticsEventType eventType, int i_enumVal, int i_val)
        {
            this.EventType = eventType;
            this.IntValue = i_val;
            this.EnumValue = i_enumVal;
        }
    }
    public struct StringMessageAnalyticsEvent : IAnalyticsEvent
    {
        public readonly AnalyticsEventType EventType { get; }
        public int IntValue => 0;
        public int EnumValue => 0;
        public readonly string StringValue { get; }
        public AnalyticsEventValues ValuesMask => AnalyticsEventValues.StringValue;

        public StringMessageAnalyticsEvent(AnalyticsEventType eventType, string value)
        {
            this.EventType = eventType;
            this.StringValue = value;
        }
    }
    #endregion
}
