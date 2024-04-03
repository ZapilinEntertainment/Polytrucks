using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    internal sealed class PlayerMoneyEffectsHandler
    {
        private bool _awaitingMessage = false;
        private bool IsTimeForALabel => Time.time - _lastLabelTime > MIN_LABEL_DELAY;
        private float _lastLabelTime = -1f;
        private UIManager _manager;
        private UIColorsPack _colorsPack;
        private MonoMemoryPool<MoneyEffectLabel> _moneyEffectLabelPool;
        private Queue<SellOperationContainer> _delayedMessages = new Queue<SellOperationContainer>();
        private const float MIN_LABEL_DELAY = 0.1f;

        public PlayerMoneyEffectsHandler(UIManager manager, MonoMemoryPool<MoneyEffectLabel> moneyEffectLabelPool, SignalBus signalBus)
        {
            _manager = manager;
            _colorsPack = manager.ColorsPack;
            _moneyEffectLabelPool = moneyEffectLabelPool;
            signalBus.Subscribe<PlayerItemSellSignal>(OnPlayerMoneyOperation);
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
            label.Setup(info.MoneyCount, _colorsPack.GetRarityColor(info.Rarity), info.SellZonePosition, _manager.LabelsHost);
            _lastLabelTime = Time.time;
        }
    }
}
