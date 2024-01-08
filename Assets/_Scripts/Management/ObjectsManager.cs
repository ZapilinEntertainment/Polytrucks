using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    
	public sealed class ObjectsManager
	{
        private readonly CratesPack _cratesPack;
        private readonly IconsPack _iconsPack;
        private readonly UIColorsPack _colorsPack;
        private Crate.Pool _cratesPool;
        private CollectibleModel.Pool _modelsPool;
        private CratesPack.Cacher _cratesCacher;

        public ObjectsManager(Crate.Pool cratePool, CollectibleModel.Pool modelPool, CratesPack cratesPack, IconsPack iconsPack, UIColorsPack colorsPack)
        {       
            _cratesPack= cratesPack;
            _cratesPool= cratePool;
            _modelsPool= modelPool;
            _iconsPack= iconsPack;
            _colorsPack= colorsPack;

            _cratesCacher = _cratesPack.CreateCacher();
        }

        public Crate CreateCrate(VirtualCollectable collectable) => CreateCrate(collectable.CollectableType, collectable.Rarity);
        public Crate CreateCrate(CollectableType type, Rarity rarity)
        {
            var crate =  _cratesPool.Spawn();
            crate.Setup(type, rarity, GetCollectibleModel(type, rarity));  
            return crate;
        }
        public CollectibleModel GetCollectibleModel(VirtualCollectable item) => GetCollectibleModel(item.CollectableType, item.Rarity);
        public CollectibleModel GetCollectibleModel(CollectableType type, Rarity rarity)
        {
            var model = _modelsPool.Spawn();
            model.Setup(_cratesPack.GetCrateModel(rarity), _iconsPack.GetIcon(type), _cratesCacher.GetIconColor(type, rarity));
            return model;
        }
	}
}
