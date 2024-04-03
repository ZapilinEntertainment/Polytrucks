using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SteerAdviceTracker : PlayerAdviceTracker
	{
        private float _affectionValue = 0f;
        public override TutorialAdviceID AdviceID => TutorialAdviceID.SteerAdvice;
        protected override bool CheckCondition(float deltaTime)
        {
            var vehicle = PlayerController.ActiveVehicle;
            if (vehicle != null && Mathf.Abs(vehicle.SteerValue) != 0f)
            {
                _affectionValue += deltaTime;
                if (_affectionValue > 1f) return true;
            }
            return false;
        }
    }
}
