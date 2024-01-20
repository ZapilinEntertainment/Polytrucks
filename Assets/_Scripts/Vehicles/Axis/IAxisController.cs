using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IAxisController 
	{
		public static IAxisController Default { get; private set; }

		public float SteerValue { get; }
		public float MaxSteerAngle { get; }
		public float GasValue { get; }
        public float MaxEngineSpeed { get; }
        public float CalculatePowerEffort(float pc);

        public class DefaultController : IAxisController
        {
            public float SteerValue => 0f;
            public float MaxSteerAngle => 45f;
            public float GasValue => 0f;
            public float MaxEngineSpeed => GameConstants.MAX_SPEED;
            public float CalculatePowerEffort(float pc) => 0f;
        }
    }
}
