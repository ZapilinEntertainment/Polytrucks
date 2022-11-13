using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public sealed class MoneyLabel : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text _label;

        private void Start()
        {
            var moneyManager = MoneyManager.Instance;
            _label.text = moneyManager.Money.ToString();
            moneyManager.OnMoneyChanged += OnMoneyChanged;
        }
        private void OnMoneyChanged(int x)
        {
            _label.text = x.ToString();
        }
    }
}
