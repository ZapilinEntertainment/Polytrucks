using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    [RequireComponent(typeof(Collider))]
    public class ItemHolder : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private GameObject _model;
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag(GameConstants.TRUCK_TAG))
            {
                var truck = col.GetComponent<TruckController>();
                if (truck?.TryCollectionItem(_item) ?? false)
                {
                    //return to pool
                    Destroy(gameObject);
                }
            }
        }
    }
}
