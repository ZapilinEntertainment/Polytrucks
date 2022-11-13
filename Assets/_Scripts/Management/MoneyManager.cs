using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public sealed class MoneyManager : MonoBehaviour
    {
        public int Money { get; private set; }
        public System.Action<int> OnMoneyChanged;

        private static MoneyManager s_current;
        public static MoneyManager Instance
        {
            get
            {
                if (s_current == null)
                {
                    var g = new GameObject("Money Manager Instance");
                    s_current = g.AddComponent<MoneyManager>();
                }
                return s_current;
            }
        }


        private void Awake()
        {
            s_current = this;
            Money = 0;
        }

        public void AddMoney(int x)
        {
            Money += x;
            OnMoneyChanged?.Invoke(Money);
        }
    }
}