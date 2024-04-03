using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{

    public class SupplyQuest : QuestBase
    {
        public override bool UseMarkerTracking => true;
        public override QuestType QuestType => QuestType.Supply;
        public DeliveryPoint DeliveryPoint { get; protected set; }
        protected int _deliveredItemCount, _requiredItemsCount;
        protected VirtualCollectable _deliveryItem;


        public SupplyQuest(DeliveryPoint endPoint, VirtualCollectable itemType, int requiredCount)
        {
            _deliveredItemCount = 0;
            _requiredItemsCount = requiredCount;
            _deliveryItem = itemType;
            DeliveryPoint = endPoint;
            DeliveryPoint.SellZone.OnItemSoldEvent += OnItemReceived;

        }

        private void OnItemReceived(VirtualCollectable item)
        {
            if (item == _deliveryItem)
            {
                _deliveredItemCount++;
                OnProgressionChangedEvent?.Invoke();
                TryCompleteQuest();
            }
        }

        public override bool CheckConditions()
        {
            return _deliveredItemCount >= _requiredItemsCount;
        }

        override protected void StopQuest()
        {
            if (!DeliveryPoint.IsDisposed) DeliveryPoint.SellZone.OnItemSoldEvent -= OnItemReceived;
            base.StopQuest();
        }
        public override Vector3 GetWorldPosition() => DeliveryPoint.MarkerPosition;
        public override IQuestMessage FormProgressionMsg() => new DeliveryQuestMessage(_deliveredItemCount, _requiredItemsCount);
        public override IQuestMessage FormNameMsg() => new SupplyPointMessage(DeliveryPoint);
    }
}
