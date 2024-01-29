using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class AnimatedProgressionBar : ProgressionBar
	{
		[SerializeField] private UnityEngine.UI.Image _animatedProgressionBar;
        [SerializeField] private float _duration = 0.5f;
        private bool _animate = false;

        protected override void i_SetProgress(float percent)
        {
            base.i_SetProgress(percent);
            _animatedProgressionBar.DOFillAmount(percent, _duration);
        }
    }
}
