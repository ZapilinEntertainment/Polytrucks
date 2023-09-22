using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MonoInstaller_0 : MonoInstaller
    {
        [SerializeField] private SessionMaster _sessionMaster;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private CameraController _cameraController;
        public override void InstallBindings()
        {
            Container.Bind<SessionMaster>().FromInstance( _sessionMaster ).AsSingle();
            Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle();
            Container.Bind<LevelManager>().FromInstance(_levelManager).AsSingle();
            Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();

            Container.Bind<ColliderListSystem>().AsSingle().Lazy();

            InstallSignals();
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
    }
}