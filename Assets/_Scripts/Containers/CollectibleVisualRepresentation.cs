using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CollectibleVisualRepresentation
    {
        public CollectableType Type { get; private set; }
        private CollectibleModel _model;

        public CollectibleVisualRepresentation(CollectableType type, CollectibleModel model)
        {
            Type = type;
            _model = model;
        }

        public void Dispose()
        {
            _model.Dispose();
            _model = null;
        }
    }
}
