using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class Garage : PlayerTrigger
	{
        private SignalBus _signalBus;
        [Inject]
        public void Inject(SignalBus signalBus)
        {
            _signalBus= signalBus;
        }
        protected override void OnPlayerEnter(PlayerController player)
        {
            base.OnPlayerEnter(player);
            _signalBus.Fire(new GarageOpenedSignal(this));
            Debug.Log("here");
        }
    }
}
