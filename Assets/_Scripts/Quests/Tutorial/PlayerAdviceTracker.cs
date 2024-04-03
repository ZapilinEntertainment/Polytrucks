using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

namespace ZE.Polytrucks {
	public abstract class PlayerAdviceTracker : TutorialAdvice, IDynamicLocalizer
	{

        public PlayerController PlayerController { get; private set; }

		[Inject]
		public void Setup(PlayerController playerController)
		{
			PlayerController= playerController;
		}

		private void FixedUpdate()
		{
			if (CheckCondition(Time.fixedDeltaTime))
			{
				AdviceLabel.DOFade(0f, 1f).OnComplete(OnComplete);
			}
		}

		
		protected abstract bool CheckCondition(float deltaTime);

    }
}
