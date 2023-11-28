using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Collider))]
	public class PlayerTrigger : MonoBehaviour
	{
        private bool _isPlayerInside = false;
        private int _controllingID = -1;
        private ColliderListSystem _collidersList;
        private Collider _trigger;
        private PlayerController _player;
        public bool IsPlayerInside => _isPlayerInside;
        public Action OnPlayerExitEvent;
        public Action<PlayerController> OnPlayerEnterEvent;


        [Inject]
        public void Inject(ColliderListSystem listSystem) { _collidersList = listSystem; }

        virtual protected void Awake()
        {
            _trigger = GetComponent<Collider>();
            _trigger.isTrigger = true;
        }

        public bool IsPlayerFullyInside()
        {
            if (_isPlayerInside)
            {
                var triggerBounds = _trigger.bounds;
                var playerBounds = _player.GetPlayerBounds();
                foreach (var bound in playerBounds)
                {
                    if (!triggerBounds.Contains(bound))
                    {
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        public void SetActivity(bool x) => _trigger.enabled = x;

        private void OnTriggerEnter(Collider other)
        {
            int id = other.GetInstanceID();
            if (_collidersList.TryDefineAsPlayer(id, out var player))
            {
                _controllingID = id;
                _player = player;
                OnPlayerEnter(player);
            }
        }
        virtual protected void OnPlayerEnter(PlayerController player)
        {
            _isPlayerInside = true;
            OnPlayerEnterEvent?.Invoke(_player);
        }
        private void OnTriggerExit(Collider other)
        {
            if (_isPlayerInside && other.GetInstanceID() == _controllingID)
            {
                _isPlayerInside = false;
                _controllingID = -1;
                OnPlayerExitEvent?.Invoke();
            }
        }
    }
}