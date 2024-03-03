using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class TutorialSystem : MonoBehaviour
	{

		[SerializeField] private TutorialAdvice[] _advices;
		private IntCompleteMask _completeMask;
		private ITutorialCompletionData _tutorialData;

		[Inject]
		public void Inject(IAccountDataAgent accountData)
		{
			_tutorialData = accountData.PlayerDataAgent.TutorialData;
		}

        private void Start()
        {
			_completeMask = _tutorialData.GetTutorialCompleteMask();

            int count = _advices.Length;
            if (_tutorialData.IsTutorialFullyCompleted)
			{				
				if (count> 0)
				{
					for(int i = 0; i< count; i++)
					{
						_advices[i].OnComplete();
					}
				}
				Destroy(this);
			}
			else
			{
                for (int i = 0; i < count; i++)
                {
					var advice = _advices[i];
					if (advice != null)
					{
						if (_completeMask.IsFlagCompleted((int)advice.AdviceID)) advice.OnComplete();
						else
						{
							var id = advice.AdviceID;
							advice.OnCompleteEvent += () => OnAdviceCompleted(id);
						}
					}
                }
            }
        }

		private void OnAdviceCompleted(TutorialAdviceID id)
		{
			_tutorialData.MarkTutorialStepAsCompleted(id);
			_completeMask = _tutorialData.GetTutorialCompleteMask();
			if (_tutorialData.IsTutorialFullyCompleted) Destroy(this);
		}
    }
}
