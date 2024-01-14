using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class AppearFrameEffect : InterfaceHitEffect
	{
		[SerializeField] private UnityEngine.UI.Image _frame;
        [SerializeField] private float _offset = 10f, _duration = 0.25f;
        private bool _isPlaying = false;
        private float _progress = 0f;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = _frame.rectTransform;
        }
        override public void Hit()
        {
            _progress = 0f;
            if (!_isPlaying)
            {
                _frame.enabled = true;
                _isPlaying = true;
                _rectTransform.offsetMax = Vector2.zero;
                _rectTransform.offsetMin = Vector2.zero;
            }
        }

        private void Update()
        {
            if (_isPlaying)
            {
                _progress = Mathf.MoveTowards(_progress, 1f, Time.deltaTime / _duration);
                Vector2 offset = Vector2.one * _offset * (_progress - 1f);
                _rectTransform.offsetMin = -offset;
                _rectTransform.offsetMax = offset;

                if (_progress == 1f)
                {
                    _isPlaying = false;
                    _frame.enabled = false;
                }
            }
        }

        public override void StopEffect()
        {
            if (_isPlaying)
            {
                _isPlaying = false;
                _frame.enabled = false;
            }
        }
    }
}
