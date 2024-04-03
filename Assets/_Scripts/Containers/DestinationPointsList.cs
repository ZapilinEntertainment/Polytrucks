using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class DestinationPointsList : MonoBehaviour, IQuestDataContainer
	{
		[SerializeField] private Transform[] _destinationPoints;

		public bool TryGetPoint(int index, out VirtualPoint point)
		{
			if (index > -1 && index < _destinationPoints.Length)
			{
				var transform = _destinationPoints[index];
				if (transform != null)
				{
					point = new VirtualPoint(transform);
					return true;
				}
				
			}
			point = default(VirtualPoint);
			return false;
		}
	}
}
