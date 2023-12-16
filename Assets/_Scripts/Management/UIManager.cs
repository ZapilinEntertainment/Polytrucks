using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class UIManager : SessionObject
	{
		[SerializeField] private GameObject _playerUI;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ActionPanel ActionPanel;

        private PlayerMoneyEffectsHandler _moneyEffectsHandler;
        private AppearingLabel.Pool _appearingLabels;
        private Camera _camera;
        private ChoicePopup _choicePopup;
        private MonoMemoryPool<ObjectScreenMarker> _markersPool;

        public UIColorsPack ColorsPack { get; private set; }
        public Transform LabelsHost { get; private set; }
        public Canvas RootCanvas => _canvas;
        [field: SerializeField] public Transform MarkersHost { get; private set; }
        [field: SerializeField] public Transform PopupHost { get; private set; }
        [field: SerializeField] public Transform AppearingLabelsHost { get; private set; }

        [Inject]
        public void Inject(UIColorsPack colorsPack, MoneyEffectLabel.Pool moneyEffectPool, SignalBus signalBus, CameraController cameraController,
            AppearingLabel.Pool appearLabelsPool, ChoicePopup choicePopup, ObjectScreenMarker.Pool markersPool)
        {
            ColorsPack = colorsPack;
            _moneyEffectsHandler = new PlayerMoneyEffectsHandler(this, moneyEffectPool, signalBus);
            _camera = cameraController.Camera;
            _appearingLabels = appearLabelsPool;
            _choicePopup = choicePopup;
            _markersPool = markersPool;

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
        public int ShowPayPanel(ActionContainer container) => ActionPanel.Show(container);
        public void HidePayPanel(int actionId) => ActionPanel.Hide(actionId);
        public void ShowAppearLabel(Vector3 worldPos, string text)
        {
            var label = _appearingLabels.Spawn();
            label.Setup(_camera.WorldToScreenPoint(worldPos), text);
        }
        public ObjectScreenMarker GetObjectMarker() => _markersPool.Spawn();
        #endregion
    }
}
