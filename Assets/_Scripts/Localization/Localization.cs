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
        Refuse_AlreadyHaveSuchQuest
    }
    internal interface ILocalizer
    {
        public string GetLocalizedString(LocalizedString localizedString);
        public string FormDeliveryAddress(PointOfInterest poi);
        public string FormSupplyAddress(PointOfInterest poi);
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
        public Action<LocalizationLanguage> OnLocaleChangedEvent;


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
    }
}
