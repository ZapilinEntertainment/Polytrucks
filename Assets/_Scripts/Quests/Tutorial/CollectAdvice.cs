using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class CollectAdvice : TutorialAdvice, IDynamicLocalizer
	{
		[SerializeField] private Crate _crate;
        public override TutorialAdviceID AdviceID => TutorialAdviceID.CollectAdvice;

        private void Start()
        {
            _crate.OnCollectedEvent += OnComplete;
        }

        
    }
}
