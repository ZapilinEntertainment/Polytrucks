using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class UIManager : SessionObject
	{
		[SerializeField] private GameObject _playerUI;
        [SerializeField] private Canvas _canvas;
        
        private PlayerMoneyEffectsHandler _moneyEffectsHandler;

        public UIColorsPack ColorsPack { get; private set; }
        public Transform LabelsHost { get; private set; }
        [field: SerializeField] public ActionPanel ActionPanel { get; private set; }
        [field: SerializeField] public Transform PopupHost { get; private set; }

        [Inject]
        public void Inject(UIColorsPack colorsPack, MoneyEffectLabel.Pool moneyEffectPool, SignalBus signalBus)
        {
            ColorsPack = colorsPack;
            _moneyEffectsHandler = new PlayerMoneyEffectsHandler(this, moneyEffectPool, signalBus);

            LabelsHost = _canvas.transform;
        }
        public void InstallElements(DiContainer container)
        {

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
