using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private MoneyEffectLabel _moneyEffectPrefab;
        [SerializeField] private ChoicePopup _choicePopupPrefab;
        [SerializeField] private AppearingLabel _appearingLabelPrefab;
        [SerializeField] private ObjectScreenMarker _screenMarkerPrefab;
        public override void InstallBindings()
        {            
            // all bindings will work only inside ui game object context;

            Container.BindMemoryPool<MoneyEffectLabel, MoneyEffectLabel.Pool>()
            .WithInitialSize(8).WithMaxSize(32)
            .FromComponentInNewPrefab(_moneyEffectPrefab).
            UnderTransform(_uiManager.AppearingLabelsHost);

            Container.BindMemoryPool<AppearingLabel, AppearingLabel.Pool>()
            .WithInitialSize(4).WithMaxSize(12)
            .FromComponentInNewPrefab(_appearingLabelPrefab).
            UnderTransform(_uiManager.AppearingLabelsHost);

            Container.BindMemoryPool<ObjectScreenMarker, ObjectScreenMarker.Pool>()
            .WithInitialSize(2).WithMaxSize(8)
            .FromComponentInNewPrefab(_screenMarkerPrefab).
            UnderTransform(_uiManager.MarkersHost);


            Container.Bind<ChoicePopup>().FromComponentInNewPrefab(_choicePopupPrefab).UnderTransform(_uiManager.PopupHost).AsCached();

            Debug.Log("ui install");
        }
    }
}
