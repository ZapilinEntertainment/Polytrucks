using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.AccountData {
	public class PlayerDataInstaller
	{
        protected readonly DiContainer _container;
        public PlayerDataInstaller(DiContainer container)
        {
            _container = container;            
        }
        public void InstallBindings()
        {
            _container.BindInterfacesAndSelfTo<RewardService>().AsCached();
            InstallPlayerDataSave();
            _container.Bind<IPlayerDataAgent>().To<PlayerData>().FromNew().AsCached();
            InstallAccountController();
        }

        virtual protected void InstallPlayerDataSave()
        {
            _container.Bind<IPlayerDataSave>().To<PlayerDataSave>().FromInstance(PlayerDataSave.Default).AsCached();            
        }
        virtual protected void InstallAccountController()
        {
            _container.BindInterfacesAndSelfTo<AccountController>().AsCached();
        }
    }
}
