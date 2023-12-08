using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [System.Serializable]
    public class DeliveryPoint
    {
        public bool IsDisposed { get; private set; } = false;
        [field:SerializeField] public SellZoneBase SellZone { get; private set; }
        [field: SerializeField] public PointOfInterest PointOfInterest { get; private set; }
        [field: SerializeField] public Vector3 MarkerPosition { get; private set; }
    }
}
