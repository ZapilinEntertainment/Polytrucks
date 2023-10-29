using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	[RequireComponent(typeof(Collider))]
	public abstract class TradeZone : MonoBehaviour
	{
        [SerializeField] private SwitchableRenderer[] _switchableRenderers;
        [SerializeField] private bool _tradeToNowhere = true;
        [SerializeField] private Collider _trigger;
        protected ColliderListSystem _collidersList;
        protected bool _hasStorage = false, _isActive = true;
        protected bool TradeToNowhere => _tradeToNowhere;
        protected int FreeSlotsCount => TradeToNowhere ? int.MaxValue : (_hasStorage ? _storage.FreeSlotsCount : 0);
        protected IStorage _storage;
        public bool CanTrade => _isActive & ( _tradeToNowhere | _hasStorage);


        [Inject]
        public void Inject(ColliderListSystem collidersList)
        {
            _collidersList = collidersList;
        }
        virtual public void AssignStorage(IStorage storage)
        {
            _storage = storage;
            _hasStorage = true;
            _tradeToNowhere = false;
        }

        public void SetActivity(bool x)
        {
            _isActive = x;
            if (_switchableRenderers != null)
            {
                foreach (var renderer in _switchableRenderers) renderer.SetActivity(x);
            }
            _trigger.enabled = _isActive;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CanTrade) OnTradeTriggerEnter(other);
        }
        virtual protected void OnTriggerExit(Collider other) { }
        abstract protected void OnTradeTriggerEnter(Collider other);
    }
}
