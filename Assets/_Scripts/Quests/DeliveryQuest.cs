using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

	public class DeliveryPoint
	{
		public bool IsDisposed = false;
		public ISellZone SellZone { get; private set; }
		public PointOfInterest PointOfInterest { get; private set; }
		public Vector3 MarkerPosition { get; private set; }
    }
	public class DeliveryQuest : QuestBase
	{
		public override bool UseMarkerTracking => true;
        public DeliveryPoint DeliveryPoint { get; private set; }
        private int _deliveredItemCount, _requiredItemsCount;
        private VirtualCollectable _deliveryItem;
		

		public DeliveryQuest(DeliveryPoint endPoint, int requiredCount)
		{
			_deliveredItemCount = 0;
			_requiredItemsCount = requiredCount;
			DeliveryPoint = endPoint;
            DeliveryPoint.SellZone.OnItemSoldEvent += OnItemReceived;
		}

		private void OnItemReceived(VirtualCollectable item)
		{
			if (item.EqualsTo(_deliveryItem))
			{
				_deliveredItemCount++;
				OnProgressionChangedEvent?.Invoke();
			}
		}

        public override bool TryComplete()
        {
			IsCompleted = IsActive & (IsCompleted || _deliveredItemCount >= _requiredItemsCount);
			return IsCompleted;
        }

        public override void StopQuest()
        {
			if (!DeliveryPoint.IsDisposed) DeliveryPoint.SellZone.OnItemSoldEvent -= OnItemReceived;
			base.StopQuest();
        }
		public override Vector3 GetTargetPosition() => DeliveryPoint.MarkerPosition;
        public override IQuestMessage FormProgressionMsg() => new DeliveryQuestMessage(_deliveredItemCount, _requiredItemsCount);
		public override IQuestMessage FormNameMsg() => new DeliveryPointMessage(DeliveryPoint);
    }
}
