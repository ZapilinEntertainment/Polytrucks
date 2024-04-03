using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [System.Serializable]
    public class DeliveryPoint
    {
        public bool IsDisposed { get; private set; } = false;
        [SerializeField] private Transform Point;
        [field:SerializeField] public SellZoneBase SellZone { get; private set; }
        [field: SerializeField] public PointOfInterest PointOfInterest { get; private set; }
        public Vector3 MarkerPosition => Point.position;
    }
}
