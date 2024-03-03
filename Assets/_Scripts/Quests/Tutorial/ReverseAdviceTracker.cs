using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class ReverseAdviceTracker  : GasAdviceTracker
	{
        protected override bool GasCondition(float gas) => gas < -0.4f;
        public override TutorialAdviceID AdviceID => TutorialAdviceID.ReverseAdvice;
    }
}
