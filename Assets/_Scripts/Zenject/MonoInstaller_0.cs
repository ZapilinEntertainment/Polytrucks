using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MonoInstaller_0 : MonoInstaller
    {
        [SerializeField] private SessionMaster _sessionMaster;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ResourcesList _resourcesList;        

        public override void InstallBindings()
        {
            Container.Bind<ResourcesList>().FromInstance(_resourcesList).AsSingle();
            ResourcesInstaller.Install(Container);

            Container.Bind<SessionMaster>().FromInstance( _sessionMaster ).AsSingle();

            Container.Bind<PlayerData>().AsCached();
            Container.Bind<PlayerController>().FromInstance(_playerController).AsCached();            

            Container.Bind<LevelManager>().FromInstance(_levelManager).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();

            InstallSystems();

            InstallSignals();
            InstallFactories();
            InstallGameObjects();

            var uiElementsPack = _resourcesList.UiElementsPack;
            Container.Bind<UIElementsPack>().FromScriptableObject(uiElementsPack).AsCached();
            Container.Bind<UIManager>().FromComponentInNewPrefab(uiElementsPack.GameUiManager).AsCached().NonLazy();
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
        }
        private void InstallFactories()
        {
            PoolsInstaller.Install(Container);
            Container.Bind<ObjectsManager>().AsSingle();
            Container.BindFactory<Storage, Storage.Factory>().AsSingle();
            Container.BindFactory<StorageVisualizer, StorageVisualizer.Factory>().AsSingle();
            Container.BindFactory<ProductionModule, ProductionModule.Factory>().AsSingle();  
        }
        private void InstallGameObjects()
        {
            
        }
    }
}