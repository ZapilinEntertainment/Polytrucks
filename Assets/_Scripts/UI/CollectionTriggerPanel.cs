using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace ZE.Polytrucks {
	public class CollectionTriggerPanel : MonoBehaviour, IPoolable, ICountTracker, IDynamicLocalizer
	{
		[SerializeField] protected Image _icon, _rarityFrame;
		[SerializeField] protected TMP_Text _countLabel, _requireInfo;
		[SerializeField] protected InterfaceHitEffect _hitEffect;
        private bool _isActive = false;
        private LocalizedString _requireInfoStringID = LocalizedString.Undefined;
        private IconsPack _iconsPack;
        private UIColorsPack _colorsPack;
        private CollectionActivatedTrigger _collectionTrigger;
        private Camera _camera;
        private Localization _localization;        
        private MonoMemoryPool<CollectionTriggerPanel> _pool;

        private void Start()
        {
            _localization.Subscribe(this);
        }

        public void StartTracking(CollectionActivatedTrigger collectionTrigger)
        {
            _collectionTrigger = collectionTrigger;
            _icon.sprite = _iconsPack.GetIcon(collectionTrigger.ItemType);
            _icon.color = _colorsPack.GetResourceIconColor(collectionTrigger.ItemType);
            _rarityFrame.color = _colorsPack.GetRarityColor(collectionTrigger.RarityConditions.MinimumRarity());

            _requireInfoStringID = LocalizedString.Undefined;
            if (collectionTrigger.TryGetInfoString(out var infoStringID))
            {
                if (!_localization.TryGetLocalizedEnum(infoStringID, out _requireInfoStringID))
                {
#if UNITY_EDITOR
                    Debug.LogWarning($"string {infoStringID} not recognized");
#endif
                }
            }
            ShowRequireInfo();

            collectionTrigger.Subscribe(this);
            _isActive = true;
        }
        private void ShowRequireInfo()
        {
            if (_requireInfoStringID != LocalizedString.Undefined)
            {
                _requireInfo.text = _localization.GetLocalizedString(_requireInfoStringID) + ':';
                _requireInfo.enabled = true;
            }
            else
            {
                _requireInfo.enabled = false;
            }
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

        public void OnLocaleChanged(LocalizationLanguage language)
        {
            if (_isActive)  ShowRequireInfo();
        }
        

        public class Pool : MonoMemoryPool<CollectionTriggerPanel>
        {
            private IconsPack _iconsPack;
            private Camera _camera;
            private UIColorsPack _colorsPack;
            private Localization _localization;
            public Pool(IconsPack iconsPack, CameraController cameraController, UIColorsPack colorsPack, Localization localization) : base()
            {
                _iconsPack = iconsPack;
                _camera = cameraController.Camera;
                _colorsPack = colorsPack;
                _localization = localization;
            }
            protected override void OnCreated(CollectionTriggerPanel item)
            {
                base.OnCreated(item);
                item._pool = this;
                item._iconsPack = _iconsPack;
                item._camera = _camera;
                item._colorsPack = _colorsPack;
                item._localization= _localization;
            }
        }
    }
}
