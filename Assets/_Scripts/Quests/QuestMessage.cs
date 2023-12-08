using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

	public interface IQuestMessage
	{
		public string ToString(Localization localization);
	}
	public readonly struct DeliveryQuestMessage : IQuestMessage
	{
		public readonly int DeliveredItemsCount, RequiredItemsCount;
		public DeliveryQuestMessage(int delivered, int total)
		{
			DeliveredItemsCount = delivered;
			RequiredItemsCount = total;
		}

		public string ToString(Localization localization) => localization.GetLocalizedString(LocalizedString.ItemsDelivered) + $": {DeliveredItemsCount}/{RequiredItemsCount}";
    }
	public readonly struct DeliveryPointMessage : IQuestMessage
	{
		public readonly DeliveryPoint DeliveryPoint;
		public DeliveryPointMessage(DeliveryPoint point) => DeliveryPoint = point;
		public string ToString(Localization localization) => localization.FormDeliveryAddress(DeliveryPoint.PointOfInterest);

    }
    public readonly struct SupplyPointMessage : IQuestMessage
    {
        public readonly DeliveryPoint DeliveryPoint;
		public SupplyPointMessage(DeliveryPoint point)
		{
			DeliveryPoint = point;
		}
        public string ToString(Localization localization) => localization.FormDeliveryAddress(DeliveryPoint.PointOfInterest);

    }
}
