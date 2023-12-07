using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class DeliveryPoint
    {
        public bool IsDisposed = false;
        public ISellZone SellZone { get; private set; }
        public PointOfInterest PointOfInterest { get; private set; }
        public Vector3 MarkerPosition { get; private set; }
    }
}
