using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public class PhysicalStorage : Storage
    {
        public struct ItemModel
        {
            public Item item;
            public GameObject model;

            public ItemModel(Item i_item, GameObject i_model)
            {
                item = i_item;
                model = i_model;
            }
        }

        [SerializeField] private Transform _modelsHost;
        protected List<ItemModel> _models;

        public override void Initialize()
        {
            _models = new List<ItemModel>();
            base.Initialize();
        }
        override public bool TryAddItem(Item item)
        {
            if (base.TryAddItem(item))
            {
                GameObject model = ObjectsManager.Instance.LoadItemModel(item);
                model.transform.parent = _modelsHost;

                int index = _itemsCount - 1;
                int verticalIndex = index / (_rowsCount * _columnCount);
                if (verticalIndex > 0) index %= (_rowsCount * _columnCount);
                int rowIndex = index / _columnCount, columnIndex = index % _columnCount;

                model.transform.localPosition = new Vector3( - (_columnCount / 2f) + 0.5f + columnIndex, verticalIndex, - (_rowsCount / 2f) + 0.5f + rowIndex) * (GameConstants.ITEM_SIZE * 1.1f);
                model.transform.localRotation = Quaternion.identity;
                _models.Add(new ItemModel( item, model));
                return true;
            }
            else return false;
        }

        override protected void RemoveItemAtPosition(int x) {
            base.RemoveItemAtPosition(x);
            GameObject model = _models[x].model;
            _models.RemoveAt(x);
            Destroy(model);
        }
    }
}
