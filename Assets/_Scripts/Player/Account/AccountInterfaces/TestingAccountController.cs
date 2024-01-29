using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.AccountData {
    public sealed class TestingAccountController : AccountController
    {
        public PlayerData PlayerData => _playerData;
        public TestingAccountController(SignalBus signalBus, GameSettings gameSettings, IPlayerDataSave dataSave) : base(signalBus, gameSettings, dataSave)
        {
        }
    }
}
