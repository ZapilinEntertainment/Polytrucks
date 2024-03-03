using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class GasAdviceTracker : PlayerAdviceTracker
	{
        private float _affectionValue = 0f;
        public override TutorialAdviceID AdviceID => TutorialAdviceID.GasAdvice;
        protected override bool CheckCondition(float deltaTime)
        {
            var vehicle = PlayerController.ActiveVehicle;
            if (vehicle != null && GasCondition(vehicle.GasValue))
            {
                _affectionValue += deltaTime;
                if (_affectionValue > 1f) return true;
            }
            return false;
        }
        protected virtual bool GasCondition(float x) => x > 0.5f;
    }
}
