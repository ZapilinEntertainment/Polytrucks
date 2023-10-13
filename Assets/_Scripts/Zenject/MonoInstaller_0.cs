using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MonoInstaller_0 : MonoInstaller
    {
        [SerializeField] private SessionMaster _sessionMaster;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private CameraController _cameraController;

        [SerializeField] private ObjectsPack _objectsPack;
        [SerializeField] private IconsPack _iconsPack;
        [SerializeField] private UIElementsPack _uiElementsPack;
        [SerializeField] private UIColorsPack _colorsPack;
        public override void InstallBindings()
        {
            InstallResourcePacks();

            Container.Bind<SessionMaster>().FromInstance( _sessionMaster ).AsSingle();
            Container.Bind<PlayerController>().FromInstance(_playerController).AsCached();
            Container.Bind<LevelManager>().FromInstance(_levelManager).AsCached();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();

            InstallSystems();

            InstallSignals();
            InstallFactories();
            InstallGameObjects();

            Container.Bind<UIManager>().FromComponentInNewPrefab(_uiElementsPack.GameUiManager);
        }

        private void InstallResourcePacks()
        {
            Container.Bind<ObjectsPack>().FromScriptableObject(_objectsPack).AsSingle();
            Container.Bind<IconsPack>().FromScriptableObject(_iconsPack).AsSingle();
            Container.Bind<UIElementsPack>().FromScriptableObject(_uiElementsPack).AsSingle();
            Container.Bind<UIColorsPack>().FromScriptableObject(_colorsPack).AsSingle();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SessionStartSignal>();
            Container.DeclareSignal<SessionStopSignal>();
            Container.DeclareSignal<SessionPauseSignal>();
            Container.DeclareSignal<SessionResumeSignal>();
            Container.DeclareSignal<CameraViewPointSetSignal>();
        }

        private void InstallSystems()
        {
            Container.Bind<ColliderListSystem>().AsCached();
            Container.Bind<CollisionHandleSystem>().AsCached();
            Container.Bind<SaveManager>().AsCached();
            Container.Bind<TradeSystem>().AsCached();
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