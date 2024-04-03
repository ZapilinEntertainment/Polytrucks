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
        public string GetLocalizedTutorialAdvice(TutorialAdviceID advice);

        public string FormDeliveryAddress(PointOfInterest poi);
        public string FormSupplyAddress(PointOfInterest poi);
        public string GetParameterName(TruckParameterType parameter);
        public string GetTruckName(TruckID truckID);
    }


    public class Localization
    {
        private IGamePreferences _gamePreferences;
        private ILocalizer i_localizer;
        public LocalizationLanguage Language { get; private set; }
        public string GetLocalizedString(LocalizedString localizedString) => i_localizer.GetLocalizedString(localizedString);
        public string GetLocalizedString(string name)
        {
            if (Enum.TryParse(typeof(LocalizedString), name, out var key))
            {
                return GetLocalizedString((LocalizedString)key);
            }
            else return name;
        }
        public string GetLocalizedTutorialAdvice(TutorialAdviceID advice) => i_localizer.GetLocalizedTutorialAdvice(advice);

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


        public Localization(IAccountDataAgent dataAgent)
        {
            _gamePreferences = dataAgent.GamePreferences;
            i_SetLanguage(_gamePreferences.SelectedLanguage);
        }
        public void ChangeLanguage(LocalizationLanguage lang, bool forced = false)
        {
            if (forced || lang != Language)
            {
                _gamePreferences.ChangeLanguage(lang);
                i_SetLanguage(lang);
            }
        }
        private void i_SetLanguage(LocalizationLanguage lang)
        {
            Language = lang;
            if (Language == LocalizationLanguage.Russian)
            {
                i_localizer = new Localizer_RUS();
            }
            else
            {
                i_localizer = new Localizer_ENG();
            }
            OnLocaleChangedEvent?.Invoke();
        }

        public string FormDeliveryAddress(PointOfInterest poi) => i_localizer.FormDeliveryAddress(poi);
        public string FormSupplyAddress(PointOfInterest poi) => i_localizer.FormSupplyAddress(poi);
        public string GetParameterName(TruckParameterType parameter) => i_localizer.GetParameterName(parameter);
        public string GetTruckName(TruckID truckID) => i_localizer.GetTruckName(truckID);

        public void Subscribe(IDynamicLocalizer localizer) => OnLocaleChangedEvent += localizer.OnLocaleChanged;
        public void Unsubscribe(IDynamicLocalizer localizer) => OnLocaleChangedEvent-= localizer.OnLocaleChanged;
    }

   
   
}
