using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace ZE.Polytrucks {
	public class CollectionTriggerPanel : MonoBehaviour, IPoolable, ICountTracker
	{
		[SerializeField] protected Image _icon, _rarityFrame;
		[SerializeField] protected TMP_Text _countLabel;
		[SerializeField] protected InterfaceHitEffect _hitEffect;
        private bool _isActive = false;
        private IconsPack _iconsPack;
        private UIColorsPack _colorsPack;
        private CollectionActivatedTrigger _collectionTrigger;
        private Camera _camera;
        private MonoMemoryPool<CollectionTriggerPanel> _pool;


        public void StartTracking(CollectionActivatedTrigger collectionTrigger)
        {
            _collectionTrigger = collectionTrigger;
            _icon.sprite = _iconsPack.GetIcon(collectionTrigger.ItemType);
            _icon.color = _colorsPack.GetResourceIconColor(collectionTrigger.ItemType);
            _rarityFrame.color = _colorsPack.GetRarityColor(collectionTrigger.RarityConditions.MinimumRarity());

            collectionTrigger.Subscribe(this);
            _isActive = true;
        }

        private void Update()
        {
            if (_isActive)
            {
                transform.position = _camera.WorldToScreenPoint(_collectionTrigger.Position);
            }
        }

        public void OnCountChanged(int x)
        {
            _countLabel.text = $"{_collectionTrigger.CollectedCount}/{_collectionTrigger.TargetCount}";
        }

        public void OnDespawned() {
            _isActive = false;
            if (_collectionTrigger != null) _collectionTrigger.Unsubscribe(this);
        }

        public void OnSpawned() { 
            _hitEffect.StopEffect();
        }

        public void OnTrackableDisposed()
        {
            if (_isActive)
            {
                _isActive = false;
                _pool.Despawn(this);
            }
        }

        public class Pool : MonoMemoryPool<CollectionTriggerPanel>
        {
            private IconsPack _iconsPack;
            private Camera _camera;
            private UIColorsPack _colorsPack;
            public Pool(IconsPack iconsPack, CameraController cameraController, UIColorsPack colorsPack) : base()
            {
                _iconsPack = iconsPack;
                _camera = cameraController.Camera;
                _colorsPack = colorsPack;
            }
            protected override void OnCreated(CollectionTriggerPanel item)
            {
                base.OnCreated(item);
                item._pool = this;
                item._iconsPack = _iconsPack;
                item._camera = _camera;
                item._colorsPack = _colorsPack;
            }
        }
    }
}
