using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace ZE.Polytrucks {
    
	public sealed class ObjectsManager
	{
        private readonly ObjectsPack _objectsPack;
        private readonly IconsPack _iconsPack;
        private readonly UIColorsPack _colorsPack;
        private Crate.Pool _cratesPool;
        private CollectibleModel.Pool _modelsPool;        

        public ObjectsManager(Crate.Pool cratePool, CollectibleModel.Pool modelPool, ObjectsPack objectsPack, IconsPack iconsPack, UIColorsPack colorsPack)
        {       
            _objectsPack= objectsPack;
            _cratesPool= cratePool;
            _modelsPool= modelPool;
            _iconsPack= iconsPack;
            _colorsPack= colorsPack;
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
            model.Setup(_objectsPack.GetCrateModel(rarity), _iconsPack.GetIcon(type), _colorsPack.GetIconColor(rarity));
            return model;
        }
	}
}
