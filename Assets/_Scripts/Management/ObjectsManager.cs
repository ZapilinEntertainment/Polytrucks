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
        
        private Crate.Pool _cratesPool;
        private CollectibleModel.Pool _modelsPool;        

        public ObjectsManager(Crate.Pool cratePool, CollectibleModel.Pool modelPool)
        {       
            _cratesPool= cratePool;
            _modelsPool= modelPool;
        }

        public Crate CreateCrate()
        {
            var crate =  _cratesPool.Spawn();
            crate.SetModel(GetCollectibleModel(crate.CollectableType));
            return crate;
        }
        public CollectibleModel GetCollectibleModel(CollectableType type)
        {
            return _modelsPool.Spawn();
        }
        public CollectibleVisualRepresentation GetCollectibleModel(ICollectable collectible) => new CollectibleVisualRepresentation(collectible.CollectableType, GetCollectibleModel(collectible.CollectableType));
	}
}
