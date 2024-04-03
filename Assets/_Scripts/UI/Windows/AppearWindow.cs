using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class AppearWindow : MonoBehaviour
	{
		[SerializeField] protected CanvasGroup _canvasGroup;
		[SerializeField] protected float _appearTime = 0.5f;
        protected bool _isShowing = false;
        protected float _showProgress = 0f;

        public void Show()
        {
            if (!_isShowing)
            {
                _isShowing = true;
                i_Show();                
            }
        }
        virtual protected void i_Show()
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowCoroutine());
        }

        protected IEnumerator ShowCoroutine()
        {

            while (_showProgress < 1f)
            {
                _showProgress = Mathf.MoveTowards(_showProgress, 1f, Time.deltaTime / _appearTime);
                _canvasGroup.alpha = _showProgress;
                yield return null;
            }
        }
    }
}
