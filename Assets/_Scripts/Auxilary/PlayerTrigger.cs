using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Collider))]
	public class PlayerTrigger : MonoBehaviour
	{
        private bool _isPlayerInside = false;
        private int _controllingID = -1;
        private ColliderListSystem _collidersList;
        private Collider _trigger;
        public System.Action<PlayerController> OnPlayerEnterEvent;

        [Inject]
        public void Inject(ColliderListSystem listSystem) { _collidersList = listSystem; }

        private void Awake()
        {
            _trigger = GetComponent<Collider>();
            _trigger.isTrigger = true;
        }

        public void SetActivity(bool x) => _trigger.enabled = x;

        private void OnTriggerEnter(Collider other)
        {
            int id = other.GetInstanceID();
            if (_collidersList.TryDefineAsPlayer(id, out var player))
            {
                _controllingID = id;
                var link_player = player;
                OnPlayerEnterEvent?.Invoke(link_player);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (_isPlayerInside && other.GetInstanceID() == _controllingID)
            {
                _isPlayerInside = false;
                _controllingID = -1;
            }
        }
    }
}
