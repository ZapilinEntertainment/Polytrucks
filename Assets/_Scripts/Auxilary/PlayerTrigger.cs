using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Collider))]
	public class PlayerTrigger : MonoBehaviour
	{
        
        private int _insideColliderID = -1;
        private ColliderListSystem _collidersList;
        protected Collider _trigger;
        protected PlayerController _player;
        public bool IsPlayerInside { get; private set; } = false;
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
            if (IsPlayerInside)
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
                _insideColliderID = id;
                _player = player;
                OnPlayerEnter(player);
            }
        }
        virtual protected void OnPlayerEnter(PlayerController player)
        {
            IsPlayerInside = true;
            OnPlayerEnterEvent?.Invoke(_player);
        }
        private void OnTriggerExit(Collider other)
        {
            if (IsPlayerInside && other.GetInstanceID() == _insideColliderID)
            {
                IsPlayerInside = false;
                _insideColliderID = -1;
                OnPlayerExitEvent?.Invoke();
            }
        }
    }
}
