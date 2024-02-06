using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class SessionStartSignal { }
	public class SessionStopSignal { }
	public class SessionPauseSignal { }
	public class SessionResumeSignal { }
	public class CameraViewPointSetSignal {
		public readonly float HeightOffsetCf;
		public readonly Transform Point;
		public CameraViewPointSetSignal(Transform point, float heightOffsetCf = 1f)
		{
			Point = point;
			HeightOffsetCf = heightOffsetCf;
		}
	}
	public class PlayerItemSellSignal
	{
		public readonly SellOperationContainer Info;
		public PlayerItemSellSignal(SellOperationContainer container)
		{
			Info = container;
		}
	}
	public class QuestCompletedSignal
	{
		public readonly QuestBase Quest;
		public QuestCompletedSignal(QuestBase quest)
		{
			Quest = quest;
		}
	}
	public class RequestCompletedSignal
	{
		public readonly CompletedRequestReport Report;
		public RequestCompletedSignal(CompletedRequestReport report)
		{
			Report = report;
		}
	}
	public class PlayerLevelUpSignal
	{
		public readonly int Level;
		public PlayerLevelUpSignal(int level) => Level = level;
	}
	public class GarageOpenedSignal
	{
		public readonly Garage Garage;
		public GarageOpenedSignal(Garage garage) => Garage= garage;
	}
	public class GarageClosedSignal { }
}
