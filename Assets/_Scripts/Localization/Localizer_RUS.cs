using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    internal class Localizer_RUS : ILocalizer
    {
        public string GetLocalizedString(LocalizedString localizedString)
        {
            switch (localizedString)
            {
                case LocalizedString.Unlock: return "��������������";
                case LocalizedString.NotEnoughMoney: return "������������ �����";
                case LocalizedString.ItemsDelivered: return "����������";

                case LocalizedString.Ask_StopQuest: return "�������� �������?";
                case LocalizedString.Ask_BuyTruck: return "������ ������?";

                case LocalizedString.StopQuest: return "��������";
                case LocalizedString.Cancel: return "������";
                case LocalizedString.QuestStarted: return "������� �������";
                case LocalizedString.CannotLoadCargo: return "�������� ����������";
                case LocalizedString.Refuse_AlreadyHaveSuchQuest: return "������� ����� ���� ��� �������";

                case LocalizedString.Garage_SelectTruck: return "�������";
                case LocalizedString.Garage_TruckAlreadySelected: return "������������";
                case LocalizedString.Garage_TruckLocked: return "�������, ����� ��������������";

                case LocalizedString.RequestZone_RebuildBridge: return "�������� ����";
                case LocalizedString.RequestZone_RebuildMine: return "��������� �����";
                case LocalizedString.RequestZone_LaunchLumbermill: return "��������� ���������";
                case LocalizedString.RequestZone_RebuildElevator: return "�������� ����";

                default: return "<�����>";
            }
        }
        public string GetInterstitialAwareString(float time)
        {
            string ending;
            switch (time % 10)
            {
                case 1: ending = "�������"; break;
                case 2:
                case 3:
                case 4: ending = "�������"; break;
                default: ending = "������"; break;
            }
            return $"������� ����� {time} " + ending;
        }
        public string FormDeliveryAddress(PointOfInterest poi)
        {
            return $"�������� � {poi.Region} {poi.PointType}";
        }
        public string FormSupplyAddress(PointOfInterest poi)
        {
            return $"��������� {poi.Region} {poi.PointType}";
        }
        public string GetParameterName(TruckParameterType parameter)
        {
            switch (parameter)
            {
                case TruckParameterType.MaxSpeed: return "����. ��������";
                case TruckParameterType.Acceleration: return "������";
                case TruckParameterType.Mass: return "�����";
                case TruckParameterType.Passability: return "������������";
                case TruckParameterType.Capacity: return "�����������";
                default: return "<��������>";
            }
        }
        public string GetTruckName(TruckID truckID)
        {
            switch (truckID)
            {
                case TruckID.TractorRosa: return "������� ����";
                case TruckID.TruckRobert: return "�������� ������";
                case TruckID.RigCosetta: return "����� �������";
                case TruckID.CarInessa: return "����� ������";
                case TruckID.PickupCortney: return "����� ������";
                default: return "<��������>";
            }
        }
    }
}
