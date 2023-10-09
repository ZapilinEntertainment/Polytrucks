using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace ZE.Polytrucks {

    public sealed class PoolsInstaller : Installer<PoolsInstaller>
    {
        private ObjectsPack _objectsPack;
        public PoolsInstaller(ObjectsPack objectsPack)
        {
            _objectsPack = objectsPack;
        }

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Crate, Crate.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_objectsPack.CratePrefab)
                .UnderTransformGroup("crates");
            Container.BindMemoryPool<CollectibleModel, CollectibleModel.Pool>()
               .WithInitialSize(10)
               .FromComponentInNewPrefab(_objectsPack.CrateModel)
               .UnderTransformGroup("collectibleModels");
        }
    }
	public sealed class ObjectsManager
	{
        private ObjectsPack _objectsPack;
        private IconsPack _iconsPack;
        private Crate.Pool _cratesPool;
        private CollectibleModel.Pool _modelsPool;        

        public ObjectsManager(Crate.Pool cratePool, CollectibleModel.Pool modelPool, ObjectsPack objectsPack, IconsPack iconsPack)
        {       
            _objectsPack= objectsPack;
            _cratesPool= cratePool;
            _modelsPool= modelPool;
            _iconsPack= iconsPack;
        }

        public Crate CreateCrate(VirtualCollectable collectable) => CreateCrate(collectable.CollectableType, collectable.Rarity);
        public Crate CreateCrate(CollectableType type, Rarity rarity)
        {
            var crate =  _cratesPool.Spawn();
            crate.SetModel(GetCollectibleModel(type, rarity));
            return crate;
        }
        public CollectibleModel GetCollectibleModel(VirtualCollectable item) => GetCollectibleModel(item.CollectableType, item.Rarity);
        public CollectibleModel GetCollectibleModel(CollectableType type, Rarity rarity)
        {
            var model = _modelsPool.Spawn();
            model.Setup(_objectsPack.GetCrateModel(rarity), _iconsPack.GetIcon(type));
            return model;
        }
	}
}
