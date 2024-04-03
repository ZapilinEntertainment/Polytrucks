using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class RequestZoneMarkerController : MonoBehaviour, IVisibilityListener
	{
        [SerializeField] private float _visibilityDistance = 40f;
		[SerializeField] private CollectionActivatedTrigger _triggerZone;
		private bool _isVisible = false, _isDisposed = false;
		private UIManager _uiManager;
        private VisibilityController _visibilityController;
        private CollectionTriggerPanel _collectionPanel;

        public Vector3 Position { get; private set; }
        private const UpdateInterval UPDATE_INTERVAL = UpdateInterval.PerFixedFrame;

        [Inject]
		public void Inject(UIManager uiManager, VisibilityController visibilityController)
		{
			_uiManager = uiManager;
            _visibilityController = visibilityController;
		}

        private void Start()
        {
            Position = _triggerZone.Position;
            _visibilityController.AddListener(new VisibilityConditions(this, UPDATE_INTERVAL, _visibilityDistance));
            _triggerZone.OnConditionCompletedEvent += StopActivity;
        }


        void IVisibilityListener.OnBecameVisible()
        {
            if (!_isVisible & !_isDisposed)
            {
                _isVisible = true;
                _collectionPanel = _uiManager.GetCollectionTriggerPanel();
                _collectionPanel.StartTracking(_triggerZone);
            }
        }

        void IVisibilityListener.OnBecameInvisible()
        {
            if (_isVisible & !_isDisposed)
            {
                _isVisible = false;
                if (_collectionPanel != null)
                {
                    _collectionPanel.OnTrackableDisposed();
                    _collectionPanel = null;
                }
            }
        }

        private void OnDisable() => StopActivity();
        private void StopActivity()
        {
            if (!_isDisposed)
            {
                (this as IVisibilityListener).OnBecameInvisible();
                _isDisposed = true;
                if (_visibilityController != null) _visibilityController.RemoveListener(this, UPDATE_INTERVAL);
                Destroy(this);
            }
        }
    }
}
