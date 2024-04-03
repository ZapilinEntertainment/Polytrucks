using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ITutorialCompletionData 
	{
		public void MarkTutorialStepAsCompleted(TutorialAdviceID id);
		public bool IsTutorialFullyCompleted { get;}
		public IntCompleteMask GetTutorialCompleteMask();		
	}
}
