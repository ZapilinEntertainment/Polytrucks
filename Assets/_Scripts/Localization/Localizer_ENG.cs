using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    internal class Localizer_ENG : ILocalizer
    {
        public string GetLocalizedString(LocalizedString localizedString)
        {
            switch (localizedString)
            {

                default: return "<text>";
            }
        }
        public string GetInterstitialAwareString(float time) => $"Advertisement in {time} seconds";

        public string FormDeliveryAddress(PointOfInterest poi)
        {
            return $"{poi.Region} {poi.PointType} delivery";
        }
        public string FormSupplyAddress(PointOfInterest poi)
        {
            return $"{poi.Region} {poi.PointType} supply";
        }
        public string GetParameterName(TruckParameterType parameter)
        {
            return "<Parameter name>";
        }
        public string GetTruckName(TruckID truckID)
        {
            switch (truckID)
            {

                default: return "<Truck>";
            }
        }
    }
}
