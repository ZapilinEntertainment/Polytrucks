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
                case LocalizedString.Unlock: return "Разблокировать";
                case LocalizedString.NotEnoughMoney: return "Недостаточно денег";
                case LocalizedString.ItemsDelivered: return "доставлено";

                case LocalizedString.Ask_StopQuest: return "Прервать задание?";
                case LocalizedString.Ask_BuyTruck: return "Купить машину?";

                case LocalizedString.StopQuest: return "Прервать";
                case LocalizedString.Cancel: return "Отмена";
                case LocalizedString.QuestStarted: return "Задание принято";
                case LocalizedString.CannotLoadCargo: return "Погрузка невозможна";
                case LocalizedString.Refuse_AlreadyHaveSuchQuest: return "Задание этого типа уже принято";

                case LocalizedString.Garage_SelectTruck: return "Выбрать";
                case LocalizedString.Garage_TruckAlreadySelected: return "Используется";
                case LocalizedString.Garage_TruckLocked: return "Найдите, чтобы разблокировать";

                case LocalizedString.RequestZone_RebuildBridge: return "Починить мост";
                case LocalizedString.RequestZone_RebuildMine: return "Запустить шахту";
                case LocalizedString.RequestZone_LaunchLumbermill: return "Запустить лесопилку";
                case LocalizedString.RequestZone_RebuildElevator: return "Починить лифт";

                default: return "<текст>";
            }
        }
        public string GetInterstitialAwareString(float time)
        {
            string ending;
            switch (time % 10)
            {
                case 1: ending = "секунду"; break;
                case 2:
                case 3:
                case 4: ending = "секунды"; break;
                default: ending = "секунд"; break;
            }
            return $"Реклама через {time} " + ending;
        }
        public string FormDeliveryAddress(PointOfInterest poi)
        {
            return $"Доставка в {poi.Region} {poi.PointType}";
        }
        public string FormSupplyAddress(PointOfInterest poi)
        {
            return $"Снабжение {poi.Region} {poi.PointType}";
        }
        public string GetParameterName(TruckParameterType parameter)
        {
            switch (parameter)
            {
                case TruckParameterType.MaxSpeed: return "Макс. скорость";
                case TruckParameterType.Acceleration: return "Разгон";
                case TruckParameterType.Mass: return "Масса";
                case TruckParameterType.Passability: return "Проходимость";
                case TruckParameterType.Capacity: return "Вместимость";
                default: return "<Параметр>";
            }
        }
        public string GetTruckName(TruckID truckID)
        {
            switch (truckID)
            {
                case TruckID.TractorRosa: return "Трактор Роза";
                case TruckID.TruckRobert: return "Грузовик Роберт";
                case TruckID.RigCosetta: return "Тягач Козетта";
                case TruckID.CarInessa: return "Седан Инесса";
                case TruckID.PickupCortney: return "Пикап Кортни";
                default: return "<Грузовик>";
            }
        }
    }
}
