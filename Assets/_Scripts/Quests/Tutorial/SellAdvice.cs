using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class SellAdvice : TutorialAdvice
    {
        [SerializeField] private CityShop _shop;
        public override TutorialAdviceID AdviceID => TutorialAdviceID.SellAdvice;

        private void Start()
        {
            _shop.OnAnyItemSoldEvent += OnComplete;
        }
    }
}
