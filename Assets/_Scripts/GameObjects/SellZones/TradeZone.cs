using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	[RequireComponent(typeof(Collider))]
	public abstract class TradeZone : MonoBehaviour
	{
        [SerializeField] private bool _tradeToNowhere = true;
        protected ColliderListSystem _collidersList;
        protected bool _hasStorage = false;
        protected bool TradeToNowhere => _tradeToNowhere;
        protected IStorage _storage;
        public bool CanTrade => _tradeToNowhere || _hasStorage;


        [Inject]
        public void Inject(ColliderListSystem collidersList)
        {
            _collidersList = collidersList;
        }
        public void AssignStorage(IStorage storage)
        {
            _storage = storage;
            _hasStorage = true;
            _tradeToNowhere = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (CanTrade) OnTradeTrigger(other);
        }
        abstract protected void OnTradeTrigger(Collider other);
    }
}
