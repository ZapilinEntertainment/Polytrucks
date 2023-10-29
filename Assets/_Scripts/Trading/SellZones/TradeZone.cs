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
        protected bool _isActive = true;
        protected bool TradeToNowhere => _tradeToNowhere;
        virtual public bool IsOperable => _isActive;


        [Inject]
        public void Inject(ColliderListSystem collidersList)
        {
            _collidersList = collidersList;
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
            if (IsOperable) OnTradeTriggerEnter(other);
        }
        virtual protected void OnTriggerExit(Collider other) { }
        abstract protected void OnTradeTriggerEnter(Collider other);
    }
}
