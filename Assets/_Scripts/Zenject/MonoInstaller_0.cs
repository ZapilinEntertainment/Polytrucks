using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MonoInstaller_0 : MonoInstaller
    {
        [SerializeField] private bool _useTestOptions = false;
        [SerializeField] private SessionMaster _sessionMaster;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIManager _uiManagerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<SessionMaster>().FromInstance( _sessionMaster ).AsSingle();
            Container.Bind<UIManager>().FromComponentInNewPrefab( _uiManagerPrefab ).AsCached().NonLazy();

            InstallAccountInfo();
            Container.Bind<PlayerController>().FromComponentInHierarchy(false).AsCached();            

            Container.Bind<LevelManager>().FromInstance(_levelManager).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();

            InstallSystems();
            InstallSignals();
            InstallFactories();
            InstallServices();
            InstallAuxiliaries();
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
            Container.DeclareSignal<QuestCompletedSignal>();
            Container.DeclareSignal<PlayerLevelUpSignal>();
            Container.DeclareSignal<RequestCompletedSignal>();
            Container.DeclareSignal<GarageOpenedSignal>();
            Container.DeclareSignal<GarageClosedSignal>();
        }

        private void InstallSystems()
        {
            Container.Bind<ColliderListSystem>().AsCached();
            Container.Bind<CollisionHandleSystem>().AsCached();
            Container.Bind<SaveManager>().AsCached();
            Container.Bind<TradeZonesManager>().AsCached().Lazy();
            Container.Bind<Localization>().AsCached();
            Container.Bind<QuestsManager>().AsCached().Lazy();
            Container.Bind<RecoverySystem>().AsCached();            

            Container.Bind<ColouredMaterialsDepot>().AsCached().Lazy();
            Container.Bind<CollectablesSpawnManager>().AsCached().Lazy();
            Container.Bind<VisibilityController>().FromNewComponentOnNewGameObject().AsCached().Lazy();
            Container.Bind<EffectsManager>().AsCached().Lazy();
            Container.Bind<RequestZonesManager>().AsCached().Lazy();            
        }
        private void InstallFactories()
        {
            PoolsInstaller.Install(Container);
            Container.Bind<ObjectsManager>().AsSingle();
            Container.BindFactory<StorageVisualizer, StorageVisualizer.Factory>().AsSingle();
            Container.BindFactory<ProductionModule, ProductionModule.Factory>().AsSingle();  
            Container.BindFactory<UnityEngine.Object, Truck, Truck.Factory>().FromFactory<PrefabFactory<Truck>>();
            Container.BindFactory<UnityEngine.Object, Trailer, Trailer.Factory>().FromFactory<PrefabFactory<Trailer>>();
            Container.BindFactory<Truck, TrailerConnector, TrailerConnector.Factory>().AsCached();
            Container.BindFactory<Truck, TrailerConnector.Handler, TrailerConnector.Handler.Factory>().AsCached();
        }
        private void InstallServices()
        {
            Container.Bind<CachedVehiclesService>().AsCached().Lazy();
            Container.Bind<TruckSwitchService>().FromNew().AsCached().Lazy();
            Container.Bind<TruckSpawnService>().FromNew().AsCached().Lazy();
        }
        private void InstallAuxiliaries()
        {
            
        }

        private void InstallAccountInfo()
        {
            var accountDataBind = Container.Bind<IAccountDataAgent>().FromSubContainerResolve().ByMethod(PlayerDataInstall).AsCached(); // with kernel
        }
        private void PlayerDataInstall(DiContainer subcontainer)
        {
            AccountData.PlayerDataInstaller.Install(subcontainer, _useTestOptions);
        }
        
    }
}