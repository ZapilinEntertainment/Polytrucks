using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class InitPoint : MonoBehaviour
	{
        private PlayerController _player;
        private SaveManager _saveManager;

        [Inject]
        public void Setup(PlayerController playerController, SaveManager saveManager)
        {
            _player = playerController;
            _saveManager = saveManager;
        }
        private void Start()
        {
            //if (ready)
            {
                _player.Teleport(_saveManager.LoadPlayerPoint());
            }
        }
        private void OnApplicationQuit()
        {
            if (_player != null && _saveManager != null)
            {
                _saveManager.SavePlayerPoint(_player.FormVirtualPoint());
            }
        }
    }
}
