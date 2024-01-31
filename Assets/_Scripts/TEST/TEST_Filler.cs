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
            if (!Input.anyKeyDown) return;
            if (_player.ActiveVehicle != null && _player.ActiveVehicle.TryGetStorage(out var storage))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    storage.TryLoadCargo(new VirtualCollectable(CollectableType.Fruits, Rarity.Regular), 10);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    storage.TryLoadCargo(new VirtualCollectable(CollectableType.IronIngot, Rarity.Regular), 10);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    storage.TryLoadCargo(new VirtualCollectable(CollectableType.Lumber, Rarity.Regular), 10);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    storage.TryLoadCargo(new VirtualCollectable(CollectableType.SteelBeam, Rarity.Advanced), 10);
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    storage.TryLoadCargo(new VirtualCollectable(CollectableType.WoodenBeam, Rarity.Advanced), 10);
                }
            }


            if (Input.GetKeyDown(KeyCode.X))
            {
                _player.ActiveVehicle.ClearCargo();
            }

            if (_testingAccountEnabled)
            {
                if (Input.GetKeyDown(KeyCode.M)) _testingAccountController.PlayerData.AddMoney(1000);
            }
        }
    }
}
