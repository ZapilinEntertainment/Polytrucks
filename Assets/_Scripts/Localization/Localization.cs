using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
    public interface IDynamicLocalizer
    {
        public void OnLocaleChanged(LocalizationLanguage language);
    }
    public enum LocalizationLanguage : byte { Undefined, English, Russian }
    public enum LocalizedString : ushort
    {
        Undefined, Unlock, NotEnoughMoney,
        ItemsDelivered,
        Ask_StopQuest,
        StopQuest,Cancel,QuestStarted, CannotLoadCargo,
        Refuse_AlreadyHaveSuchQuest,
        RequestZone_RebuildMine, RequestZone_RebuildBridge,RequestZone_LaunchLumbermill,RequestZone_RebuildElevator,
        STRING_NOT_RECOGNISED_ERROR
    }
    internal interface ILocalizer
    {
        public string GetLocalizedString(LocalizedString localizedString);
        public string FormDeliveryAddress(PointOfInterest poi);
        public string FormSupplyAddress(PointOfInterest poi);
        public string GetParameterName(TruckParameterType parameter);
        public string GetTruckName(TruckID truckID);
    }


    public class Localization
    {
        private ILocalizer i_localizer;
        private ISaveContainer _saves;
        private ILocalizer Localizer
        {
            get
            {
                if (i_localizer == null)
                {
                    Language = _saves.LoadLocale();
                    if (Language == LocalizationLanguage.Undefined)
                    {
                        if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
                        {
                            ChangeLanguage(LocalizationLanguage.Russian, true);
                        }
                        else
                        {
                            ChangeLanguage(LocalizationLanguage.English, true);
                        }
                    }
                    else ChangeLanguage(Language, true);
                }
                return i_localizer;
            }
        }
        public LocalizationLanguage Language { get; private set; }
        public string GetLocalizedString(LocalizedString localizedString) => Localizer.GetLocalizedString(localizedString);
        public string GetLocalizedString(string name)
        {
            if (Enum.TryParse(typeof(LocalizedString), name, out var key))
            {
                return GetLocalizedString((LocalizedString)key);
            }
            else return name;
        }

        public bool TryDefineStringID(string rawName, out LocalizedString localEnum)
        {
            if (Enum.TryParse(typeof(LocalizedString), rawName, out var key))
            {
                localEnum = (LocalizedString)key;
                return true;
            }
            else
            {
                localEnum = LocalizedString.Undefined;
                return false;
            }
        }
        public LocalizedString GetStringID(string rawname)
        {
            if (TryDefineStringID(rawname, out var id)) return id;
            else return LocalizedString.STRING_NOT_RECOGNISED_ERROR;
        }
        private Action<LocalizationLanguage> OnLocaleChangedEvent;


        public Localization(SaveManager saveManager)
        {
            _saves = saveManager.SaveContainer;
        }

        public void ChangeLanguage(LocalizationLanguage lang, bool forced = false)
        {
            if (forced || lang != Language)
            {
                if (lang == LocalizationLanguage.Russian)
                {
                    i_localizer = new Localizer_RUS();
                }
                else
                {
                    i_localizer = new Localizer_ENG();
                }
                Language = lang;
                _saves.SaveLocale(Language);
                OnLocaleChangedEvent?.Invoke(Language);
            }
        }
        public void SwitchLanguage()
        {
            if (Language == LocalizationLanguage.English) ChangeLanguage(LocalizationLanguage.Russian);
            else ChangeLanguage(LocalizationLanguage.English);
        }

        public string FormDeliveryAddress(PointOfInterest poi) => Localizer.FormDeliveryAddress(poi);
        public string FormSupplyAddress(PointOfInterest poi) => Localizer.FormSupplyAddress(poi);
        public string GetParameterName(TruckParameterType parameter) => Localizer.GetParameterName(parameter);
        public string GetTruckName(TruckID truckID) => Localizer.GetTruckName(truckID);

        public void Subscribe(IDynamicLocalizer localizer) => OnLocaleChangedEvent += localizer.OnLocaleChanged;
        public void Unsubscribe(IDynamicLocalizer localizer) => OnLocaleChangedEvent-= localizer.OnLocaleChanged;
    }

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
                case LocalizedString.StopQuest: return "��������";
                case LocalizedString.Cancel: return "������";
                case LocalizedString.QuestStarted: return "������� �������";
                case LocalizedString.CannotLoadCargo: return "�������� ����������";
                case LocalizedString.Refuse_AlreadyHaveSuchQuest: return "������� ����� ���� ��� �������";

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
