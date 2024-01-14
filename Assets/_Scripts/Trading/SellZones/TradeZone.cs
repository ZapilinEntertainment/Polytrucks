using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	[RequireComponent(typeof(Collider))]
	public abstract class TradeZone : MonoBehaviour, ILateDisposable, IActivitySwitchable
	{
        [SerializeField] private SwitchableRenderer[] _switchableRenderers;
        [SerializeField] private bool _tradeToNowhere = true;
        [SerializeField] private Collider _trigger;
        protected ColliderListSystem _collidersList;
        private bool _isDisposed = false;
        protected bool _isActive = true;
        protected bool TradeToNowhere => _tradeToNowhere;
        virtual public bool IsOperable => _isActive & !_isDisposed;
        public System.Action OnTradeZoneDisposedEvent;


        [Inject]
        public void Inject(ColliderListSystem collidersList, TradeZonesManager zonesManager)
        {
            _collidersList = collidersList;
            AssignToZones(zonesManager);
        }
        abstract public void AssignToZones(TradeZonesManager manager);

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
        private void OnTriggerExit(Collider other) {
            if (!_isDisposed) OnTradeTriggerExit(other);
        }
        abstract protected void OnTradeTriggerEnter(Collider other);
        abstract protected void OnTradeTriggerExit(Collider other);

        public void LateDispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                OnTradeZoneDisposedEvent?.Invoke();
            }
        }
        public void Destroy()
        {
            SetActivity(false);
            if (isActiveAndEnabled) Destroy(gameObject);
        }
    }
}
