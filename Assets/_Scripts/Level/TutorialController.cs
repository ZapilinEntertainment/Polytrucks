using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class TutorialController : MonoBehaviour
	{
        private enum TutorialStage : byte
        {
            Undefined, SellRegularGoods, SellRefinedGoods, End
        }

		[SerializeField] private SellZoneBase _startSellZone;
        [SerializeField] private CollectZone _refineryCollectZone;
		[SerializeField] private Gates _gates;
        [SerializeField] private GameObject _lineToRefinery, _lineFromRefineryToShop;
        private TutorialStage _stage = TutorialStage.SellRegularGoods;

        private void Start()
        {
            _startSellZone.OnAnyItemSoldEvent += OnFirstItemSold;
            _refineryCollectZone.OnItemsCollectedEvent += OnRefinedItemsCollected;

            _lineToRefinery.SetActive(false);
            _lineFromRefineryToShop.SetActive(false);
        }

        private void OnFirstItemSold()
        {
            if (_stage == TutorialStage.SellRegularGoods)
            {
                _stage = TutorialStage.SellRefinedGoods;
                _startSellZone.OnAnyItemSoldEvent -= OnFirstItemSold;
                _gates.Open();
                _lineToRefinery.SetActive(true);                
            }
        }
        private void OnRefinedItemsCollected()
        {
            if (_stage == TutorialStage.SellRefinedGoods)
            {
                _stage = TutorialStage.End;
                _refineryCollectZone.OnItemsCollectedEvent-= OnRefinedItemsCollected;
                _lineFromRefineryToShop.SetActive(true);
            }
        }
    }
}
