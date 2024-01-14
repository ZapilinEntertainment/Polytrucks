using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	public sealed class PlayerDataInstaller :Installer<bool,PlayerDataInstaller>
	{
        bool _useTestModule = false;
        public PlayerDataInstaller(bool useTestModule)
        {
            _useTestModule= useTestModule;
        }
        override public void InstallBindings()
        {            
            Container.BindInterfacesAndSelfTo<PlayerData>().AsCached();
            Container.BindInterfacesAndSelfTo<RewardService>().AsCached();
            if (_useTestModule)
            {
                Container.BindInterfacesAndSelfTo<TestingAccountController>().AsCached();
            }
            else
            {
                Container.BindInterfacesAndSelfTo<AccountController>().AsCached();
            }
        }
    }

    /*
    public sealed class PlayerDataLinksInstaller
    {
        bool _useTestModule = false;
        public PlayerDataLinksInstaller(bool useTestModule) => _useTestModule = useTestModule;

        public void InstallBindings(DiContainer mainContainer) {
            var subcontainer = mainContainer.CreateSubContainer();
            subcontainer.Bind<AccountController>().AsCached();
            subcontainer.Bind<PlayerData>().AsCached();
            subcontainer.Bind<RewardService>().AsCached();  

            subcontainer.Bind<IAccountDataReader>().To<AccountController>().FromResolve();
            mainContainer.Bind<IAccountDataReader>().FromResolveGetter;

            subcontainer.Bind<IPlayerDataReader>().To<PlayerData>().FromResolve();
            mainContainer.Bind<IAccountDataReader>().AsCached();

            subcontainer.Bind<IRewarder>().To<RewardService>().FromResolve(,);
            mainContainer.Bind<IRewarder>().AsCached();

            if (_useTestModule)
            {
                subcontainer.Bind<Account_TEST_links>().AsCached();
                mainContainer.Bind<Account_TEST_links>().AsCached();
            }
        }
    }
    */
}
