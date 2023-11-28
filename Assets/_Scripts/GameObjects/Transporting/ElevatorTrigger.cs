using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[RequireComponent(typeof(Collider))]
	public sealed class ElevatorTrigger : PlayerTrigger
	{
		[SerializeField] private bool _isStartPoint = false;
		[SerializeField] private Elevator _elevator;

        protected override void OnPlayerEnter(PlayerController player)
        {
            base.OnPlayerEnter(player);
            _elevator.CallElevator(_isStartPoint);
        }
    }
}
