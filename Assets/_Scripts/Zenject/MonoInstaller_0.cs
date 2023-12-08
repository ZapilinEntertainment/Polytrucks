using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MonoInstaller_0 : MonoInstaller
    {
        [SerializeField] private SessionMaster _sessionMaster;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Trailer _trailerPrefab;
        [SerializeField] private UIManager _uiManagerPrefab;

        public override void InstallBindings()
        {            
            Container.Bind<SessionMaster>().FromInstance( _sessionMaster ).AsSingle();
            Container.Bind<UIManager>().FromComponentInNewPrefab( _uiManagerPrefab ).AsCached().NonLazy();

            Container.Bind<PlayerData>().FromNew().AsCached();
            Container.Bind<PlayerController>().FromComponentInHierarchy(false).AsCached();            

            Container.Bind<LevelManager>().FromInstance(_levelManager).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();

            InstallSystems();

            InstallSignals();
            InstallFactories();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SessionStartSignal>();
            Container.DeclareSignal<SessionStopSignal>();
            Container.DeclareSignal<SessionPauseSignal>();
            Container.DeclareSignal<SessionResumeSignal>();
            Container.DeclareSignal<CameraViewPointSetSignal>();
            Container.DeclareSignal<PlayerItemSellSignal>();
        }

        private void InstallSystems()
        {
            Container.Bind<ColliderListSystem>().AsCached();
            Container.Bind<CollisionHandleSystem>().AsCached();
            Container.Bind<SaveManager>().AsCached();
            Container.Bind<TradeZonesManager>().AsCached().Lazy();
            Container.Bind<Localization>().AsCached();
            Container.Bind<QuestsManager>().AsCached().Lazy();

            Container.Bind<ColouredMaterialsDepot>().AsCached().Lazy();
        }
        private void InstallFactories()
        {
            PoolsInstaller.Install(Container);
            Container.Bind<ObjectsManager>().AsSingle();
            Container.BindFactory<StorageVisualizer, StorageVisualizer.Factory>().AsSingle();
            Container.BindFactory<ProductionModule, ProductionModule.Factory>().AsSingle();  
            Container.BindFactory<Trailer, Trailer.Factory>().FromComponentInNewPrefab(_trailerPrefab);
        }
    }
}