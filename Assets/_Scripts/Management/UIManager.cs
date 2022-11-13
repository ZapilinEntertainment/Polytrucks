using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public enum PopupType : byte { Undefined, Money }
    public static class PopupTypeExtension
    {
        public static Icon DefinePopupIcon(this PopupType type)
        {
            switch (type)
            {
                case PopupType.Money: return Icon.MoneyIcon;
                default: return Icon.Undefined;
            }
        }
    }
    public sealed class UIManager : MonoBehaviour
    {
        private static UIManager s_current;

        [SerializeField] private ProgressBar _circleProgressBar;
        [SerializeField] private PopupNote _popupNote;

        private void Awake()
        {
            s_current = this;
        }
        private void ShowPopup(PopupType type, Vector3 worldPos, string text)
        {
            _popupNote.Show(type.DefinePopupIcon(), Camera.main.WorldToScreenPoint(worldPos), text);
        }
        private void StartProgressCircle(SellZone zone) => _circleProgressBar.TrackObject(zone);
        private void EndProgressCircle(SellZone zone) => _circleProgressBar.StopTracking(zone);

        private void OnDestroy()
        {
            if (s_current == this) s_current = null;
        }

        public static void OnMoneyCollected(Vector3 worldPos, string text) => s_current?.ShowPopup(PopupType.Money, worldPos, text);
        public static void OnStartTrading(SellZone zone) => s_current?.StartProgressCircle(zone);
        public static void OnStopTrading(SellZone zone) => s_current?.EndProgressCircle(zone);
    }
}
