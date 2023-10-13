using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class PoolsInstaller : Installer<PoolsInstaller>
    {
        private ObjectsPack _objectsPack;
        private UIElementsPack _uiElementsPack;
        public PoolsInstaller(ObjectsPack objectsPack, UIElementsPack uiElementsPack)
        {
            _objectsPack = objectsPack;
            _uiElementsPack = uiElementsPack;
        }

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Crate, Crate.Pool>()
                .WithInitialSize(8)
                .FromComponentInNewPrefab(_objectsPack.CratePrefab)
                .UnderTransformGroup("crates");
            Container.BindMemoryPool<CollectibleModel, CollectibleModel.Pool>()
               .WithInitialSize(8)
               .FromComponentInNewPrefab(_objectsPack.CrateModel)
               .UnderTransformGroup("collectibleModels");

            Container.BindMemoryPool<MoneyEffectLabel, MoneyEffectLabel.Pool>()
              .WithInitialSize(8)
              .FromComponentInNewPrefab(_uiElementsPack.MoneyEffectLabel);
        }
    }
}
