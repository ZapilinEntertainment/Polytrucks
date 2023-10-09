using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CollectibleVisualRepresentation
    {
        public CollectableType CollectableType { get; private set; }
        public Rarity Rarity { get; private set; }
        private CollectibleModel _model;

        public CollectibleVisualRepresentation(CollectableType type, Rarity rarity, CollectibleModel model)
        {
            CollectableType = type;
            _model = model;
        }

        public void Dispose()
        {
            _model.Dispose();
            _model = null;
        }
    }
}
