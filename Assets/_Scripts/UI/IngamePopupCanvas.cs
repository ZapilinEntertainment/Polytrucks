using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public sealed class IngamePopupCanvas : MonoBehaviour
    {
        public enum PopupType : byte { Undefined, Money}
        private static IngamePopupCanvas s_current;

        private void Awake()
        {
            s_current = this;
        }

        private void ShowPopup(PopupType type, Vector3 pos, string text)
        {

        }
        private void StartProgressCircle(SellZone zone)
        {

        }
        private void EndProgressCircle(SellZone zone)
        {

        }

        private void OnDestroy()
        {
            if (s_current == this) s_current = null;
        }

        public static void OnMoneyCollected(Vector3 pos, string text) => s_current?.ShowPopup(PopupType.Money, pos, text);
        public static void OnStartTrading(SellZone zone) => s_current?.StartProgressCircle(zone);
        public static void OnStopTrading(SellZone zone) => s_current?.EndProgressCircle(zone);
    }
}
