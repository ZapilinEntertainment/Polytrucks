using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class TEST_Filler : MonoBehaviour
	{
		private PlayerController _player;

        [Inject]
        public void Inject(PlayerController player) => _player = player;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _player.ActiveVehicle.LoadCargo(new VirtualCollectable(CollectableType.Fruits, Rarity.Regular), 10);
            }
        }
    }
}
