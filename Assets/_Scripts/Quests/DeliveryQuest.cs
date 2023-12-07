using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	
	public class DeliveryQuest : SupplyQuest
	{
		

		public DeliveryQuest(DeliveryPoint endPoint,VirtualCollectable itemType, int requiredCount) : base(endPoint, itemType, requiredCount) { }

        public override IQuestMessage FormProgressionMsg() => new DeliveryQuestMessage(_deliveredItemCount, _requiredItemsCount);
		public override IQuestMessage FormNameMsg() => new DeliveryPointMessage(DeliveryPoint);
    }
}
