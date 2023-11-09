using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public abstract class TradeZoneHighlight : MonoBehaviour
	{
        [SerializeField] private Transform _effectSprite;
        [SerializeField] private float _effectTime = 1f, _effectScale = .1f;
        
        private bool _playerIsInside = false, _effectIsActive = false, _playerLinkSet = false;
        protected bool _contractIsSuitable = false, _contractUpdateRequested = false, _effectCheckRequested = false;
        private float _startScale = 1f;
        protected TradeContract _activeContract;
        protected PlayerController _player;
        private ColliderListSystem _collidersList;
        private HashSet<int> _activePlayersCollider = new HashSet<int>();

        private void Awake()
        {
            _startScale = _effectSprite.localScale.x;
            SubscribeToZoneChanges();
            CheckForEffect();
        }
        abstract protected void SubscribeToZoneChanges();

        [Inject]
        public void Inject(ColliderListSystem list) => _collidersList= list;

        private void Update()
        {
            if (_contractUpdateRequested) CheckContract();
            if (_effectCheckRequested) CheckForEffect();
            if (_effectIsActive)
            {
                _effectSprite.transform.localScale = (_startScale + Mathf.PingPong(Time.time / _effectTime,  _effectScale)) * Vector3.one;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int id = other.GetInstanceID();
            if (_collidersList.TryDefineAsPlayer(id, out var player)) {

                if (!_playerLinkSet)
                {
                    _player = player;
                    _player.OnItemCompositionChangedEvent += () => _contractUpdateRequested = true;
                    _playerLinkSet= true;
                }

                _activePlayersCollider.Add(id);
                _playerIsInside = true;
                if (!_contractIsSuitable) _contractUpdateRequested = true;
                _effectCheckRequested = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (_playerIsInside)
            {
                int id = other.GetInstanceID();
                _activePlayersCollider.Remove(id);
                _playerIsInside = _activePlayersCollider.Count != 0;
                _effectCheckRequested = true;
            }
        }

        abstract protected void CheckContract();
        private void CheckForEffect()
        {
            _effectIsActive = _playerIsInside & _contractIsSuitable;
            _effectSprite.gameObject.SetActive(_effectIsActive);
            _effectCheckRequested = false;
        }
    }
}
