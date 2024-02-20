using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
    public interface IDynamicLocalizer
    {
        public void OnLocaleChanged();
    }
    public enum LocalizationLanguage : byte { Undefined, English, Russian }
    public enum LocalizedString : ushort
    {
        Undefined, Unlock, NotEnoughMoney,
        ItemsDelivered,
        Ask_StopQuest, Ask_BuyTruck,
        StopQuest,Cancel,QuestStarted, CannotLoadCargo,
        Refuse_AlreadyHaveSuchQuest,
        RequestZone_RebuildMine, RequestZone_RebuildBridge,RequestZone_LaunchLumbermill,RequestZone_RebuildElevator,
        Garage_SelectTruck, Garage_TruckAlreadySelected, Garage_TruckLocked,
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
        private Action OnLocaleChangedEvent;


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
                OnLocaleChangedEvent?.Invoke();
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

   
   
}
