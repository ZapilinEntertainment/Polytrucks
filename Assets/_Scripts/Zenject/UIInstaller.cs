using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class UIInstaller : MonoInstaller
    {
        public class ElementsResolver
        {
            // for making injection in inner classes and not instancing them until they are needed
            private readonly DiContainer Container;
            public ElementsResolver(DiContainer container)
            {
                Container = container;
            }

            public GaragePanel GaragePanel => Container.Resolve<GaragePanel>();
            public ChoicePopup ChoicePopup => Container.Resolve<ChoicePopup>();
            public ActionPanel ActionPanel => Container.Resolve<ActionPanel>();
        }

        [SerializeField] private UIManager _uiManager;
        [SerializeField] private MoneyEffectLabel _moneyEffectPrefab;        
        [SerializeField] private AppearingLabel _appearingLabelPrefab;
        [SerializeField] private ObjectScreenMarker _screenMarkerPrefab;
        [SerializeField] private CollectionTriggerPanel _collectionTriggerPanelPrefab;

        [SerializeField] private GaragePanel _garagePanelPrefab;
        [SerializeField] private ChoicePopup _choicePopupPrefab;
        [SerializeField] private ActionPanel _actionPanelPrefab;
        [SerializeField] private QuestsPanel _questsPanelPrefab;
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

            Container.BindMemoryPool<CollectionTriggerPanel, CollectionTriggerPanel.Pool>()
            .WithInitialSize(2).WithMaxSize(8)
            .FromComponentInNewPrefab(_collectionTriggerPanelPrefab).
            UnderTransform(_uiManager.PanelsHost);

            BindSinglePresenceItem(_choicePopupPrefab, _uiManager.PopupHost);
            BindSinglePresenceItem(_garagePanelPrefab, _uiManager.PanelsHost);
            BindSinglePresenceItem(_actionPanelPrefab, _uiManager.PanelsHost);
            BindSinglePresenceItem(_questsPanelPrefab, _uiManager.RootCanvas.transform);

           // Container.Bind<ChoicePopup>().FromComponentInNewPrefab(_choicePopupPrefab).UnderTransform(_uiManager.PopupHost).AsCached().Lazy();
           // Container.Bind<GaragePanel>().FromComponentInNewPrefab(_garagePanelPrefab).UnderTransform(_uiManager.PanelsHost).AsCached().Lazy();
           // Container.Bind<ActionPanel>().FromComponentInNewPrefab(_actionPanelPrefab).UnderTransform(_uiManager.PanelsHost).AsCached().Lazy();

            Container.Bind<ElementsResolver>().FromNew().AsCached().Lazy();

            Debug.Log("ui install");

            void BindSinglePresenceItem<T>(T prefab, Transform host) where T : Object => Container.Bind<T>().FromComponentInNewPrefab(prefab).UnderTransform(host).AsCached().Lazy();
        }
    }
}
