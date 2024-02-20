using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks.TestModule {
	public class TestModuleContainer : MonoBehaviour
	{
		[field: SerializeField] public bool UseTestKeys { get; private set; } = true;
		[field: SerializeField] public PlayerDataSavePreset SavePreset { get; private set; }

        private PlayerController _player;

        [Inject]
        public void Inject(PlayerController player)
        {
            _player = player;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) _player.ActiveVehicle.Teleport(_player.ActiveVehicle.FormVirtualPoint().Move(Vector3.left * 4f));
            if (Input.GetKeyDown(KeyCode.RightArrow)) _player.ActiveVehicle.Teleport(_player.ActiveVehicle.FormVirtualPoint().Move(Vector3.right * 4f));
            if (Input.GetKeyDown(KeyCode.UpArrow)) _player.ActiveVehicle.Teleport(_player.ActiveVehicle.FormVirtualPoint().Move(Vector3.forward * 4f));
            if (Input.GetKeyDown(KeyCode.DownArrow)) _player.ActiveVehicle.Teleport(_player.ActiveVehicle.FormVirtualPoint().Move(Vector3.back * 4f));
        }
    }
}
