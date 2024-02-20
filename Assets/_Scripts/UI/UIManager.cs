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
        private AppearingLabel.Pool _appearingLabels;
        private CameraController _camera;
        private PanelsManager _panelsManager;
        private MonoMemoryPool<ObjectScreenMarker> _markersPool;
        private MonoMemoryPool<CollectionTriggerPanel> _collectionTriggerPanels;

        public UIColorsPack ColorsPack { get; private set; }
        public Transform LabelsHost { get; private set; }
        public Canvas RootCanvas => _canvas;
        [field: SerializeField] public Transform MarkersHost { get; private set; }
        [field: SerializeField] public Transform PopupHost { get; private set; }
        [field: SerializeField] public Transform AppearingLabelsHost { get; private set; }
        [field: SerializeField] public Transform PanelsHost { get; private set; }

        [Inject]
        public void Inject(UIColorsPack colorsPack, MoneyEffectLabel.Pool moneyEffectPool, SignalBus signalBus, CameraController cameraController,
            AppearingLabel.Pool appearLabelsPool, ObjectScreenMarker.Pool markersPool, CollectionTriggerPanel.Pool collectionTriggersPool,
            UIInstaller.ElementsResolver elementsResolver
            )
        {
            ColorsPack = colorsPack;
            _moneyEffectsHandler = new PlayerMoneyEffectsHandler(this, moneyEffectPool, signalBus);
            _camera = cameraController;
            _appearingLabels = appearLabelsPool;
            _panelsManager = new PanelsManager(elementsResolver, signalBus);
            _markersPool = markersPool;
            _collectionTriggerPanels = collectionTriggersPool;

            LabelsHost = _canvas.transform;
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

        #region ui functions
        public int ShowActionPanel(ActionContainer container) => _panelsManager.OpenActionPanel(container);
        public void HideActionPanel(int actionId) => _panelsManager.CloseActionPanel(actionId);
        public void ShowAppearLabel(Vector3 worldPos, string text)
        {
            var label = _appearingLabels.Spawn();
            label.Setup(_camera.WorldToScreenPoint(worldPos), text);
        }
        public ObjectScreenMarker GetObjectMarker() => _markersPool.Spawn();
        public CollectionTriggerPanel GetCollectionTriggerPanel() => _collectionTriggerPanels.Spawn();
        #endregion
        #region panels
        #endregion
    }
}
