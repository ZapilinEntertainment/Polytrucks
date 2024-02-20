using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZE.Polytrucks.AccountData;

namespace ZE.Polytrucks.TestModule {
	public sealed class TestPlayerDataInstaller : PlayerDataInstaller
	{
        private readonly TestModuleContainer _testContainer;
        public TestPlayerDataInstaller(DiContainer container, TestModuleContainer testData) :base(container) { 
            _testContainer = testData;
        }

        protected override void InstallPlayerDataSave()
        {
            if (_testContainer.SavePreset == null) base.InstallPlayerDataSave();
            else
            {
                var saveInstance = new PlayerDataSave(_testContainer.SavePreset);
                _container.Bind<IPlayerDataSave>().FromInstance(saveInstance).AsCached();
            }
        }
        protected override void InstallAccountController()
        {
            if (_testContainer.UseTestKeys) _container.BindInterfacesAndSelfTo<TestingAccountController>().AsCached();
            else base.InstallAccountController();
        }
    }
}
