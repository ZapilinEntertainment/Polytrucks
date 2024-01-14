using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZE.Polytrucks.AccountData;

namespace ZE.Polytrucks {
	public sealed class TEST_Filler : MonoBehaviour
	{
        private PlayerController _player;
        private IAccountDataAgent _accountDataAgent;

        private bool _testingAccountEnabled = false;
        private TestingAccountController _testingAccountController;

        [Inject]
        public void Inject( PlayerController player, IAccountDataAgent accountDataAgent) {
            _player = player;
            _accountDataAgent = accountDataAgent;
        }
        private void Start()
        {
            _testingAccountController = (_accountDataAgent as AccountData.TestingAccountController);
            _testingAccountEnabled = _testingAccountController != null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _player.ActiveVehicle.LoadCargo(new VirtualCollectable(CollectableType.Fruits, Rarity.Regular), 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _player.ActiveVehicle.LoadCargo(new VirtualCollectable(CollectableType.Metals, Rarity.Regular), 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _player.ActiveVehicle.LoadCargo(new VirtualCollectable(CollectableType.Lumber, Rarity.Regular), 10);
            }

            if (_testingAccountEnabled)
            {
                if (Input.GetKeyDown(KeyCode.M)) _testingAccountController.PlayerData.AddMoney(1000);
            }
        }
    }
}
