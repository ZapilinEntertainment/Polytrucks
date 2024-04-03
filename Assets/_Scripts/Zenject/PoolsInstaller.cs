using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class PoolsInstaller : Installer<PoolsInstaller>
    {
        private CratesPack _cratesPack;
        private UIElementsPack _uiElementsPack;
        public PoolsInstaller(CratesPack cratespack)
        {
            _cratesPack = cratespack;
        }

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Crate, Crate.Pool>()
                .WithInitialSize(8)
                .FromComponentInNewPrefab(_cratesPack.CratePrefab)
                .UnderTransformGroup("crates");
            Container.BindMemoryPool<CollectibleModel, CollectibleModel.Pool>()
               .WithInitialSize(8)
               .FromComponentInNewPrefab(_cratesPack.CrateModel)
               .UnderTransformGroup("collectibleModels");

          
        }
    }
}
