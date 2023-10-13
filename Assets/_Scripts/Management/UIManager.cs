using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class UIManager : SessionObject
	{
        private sealed class PlayerMoneyEffectsHandler
        {
            private bool _awaitingMessage = false;
            private bool IsTimeForALabel => Time.time - _lastLabelTime > MIN_LABEL_DELAY;
            private float _lastLabelTime = -1f;
            private UIColorsPack _colorsPack;
            private MonoMemoryPool<MoneyEffectLabel> _moneyEffectLabelPool;
            private Queue<SellOperationContainer> _delayedMessages;
            private const float MIN_LABEL_DELAY = 1f;

            public PlayerMoneyEffectsHandler(UIColorsPack colorsPack, MonoMemoryPool<MoneyEffectLabel> moneyEffectLabelPool)
            {
                _colorsPack = colorsPack;
                _moneyEffectLabelPool = moneyEffectLabelPool;
            }

            public void OnPlayerMoneyOperation(PlayerItemSellSignal args)
            {
                if (!_awaitingMessage && IsTimeForALabel)
                {
                    ShowMessage(args.Info);
                }
                else
                {
                    _delayedMessages.Enqueue(args.Info);
                    _awaitingMessage = true;
                }
            }

            public void Update()
            {
                if (_awaitingMessage)
                {
                    if (IsTimeForALabel && _delayedMessages.TryDequeue(out var info))
                    {
                        ShowMessage(info);
                        _awaitingMessage = _delayedMessages.Count > 0;
                    }
                }
            }
            private void ShowMessage(SellOperationContainer info)
            {
                var label = _moneyEffectLabelPool.Spawn();
                label.Setup(info.MoneyCount, _colorsPack.GetRarityColor(info.Rarity));
                _lastLabelTime = Time.time;
            }
        }

		[SerializeField] private GameObject _playerUI;
        private PlayerMoneyEffectsHandler _moneyEffectsHandler;
        

        [Inject]
        public void Inject(UIColorsPack colorsPack, MoneyEffectLabel.Pool moneyEffectPool)
        {
            _moneyEffectsHandler = new PlayerMoneyEffectsHandler(colorsPack, moneyEffectPool);
        }

        private void Start()
        {
            _signalBus.Subscribe<PlayerItemSellSignal>(_moneyEffectsHandler.OnPlayerMoneyOperation);
        }
        private void Update()
        {
            if (GameSessionActive) _moneyEffectsHandler.Update();
        }

        public override void OnSessionStart()
        {
            base.OnSessionStart();
			_playerUI.SetActive(true);
        }
        public override void OnSessionEnd()
        {
            base.OnSessionEnd();
			_playerUI.SetActive(false);
        }

        
    }
}
