using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polytrucks
{
    public sealed class PopupNote : MonoBehaviour
    {
        [SerializeField] private float _showTime = 1f, _stepPercent = 0.05f;
        [SerializeField] private Image _icon;
        [SerializeField] private TMPro.TMP_Text _label;
        [SerializeField] private CanvasGroup _canvasGroup;
        private bool _isShowing = false;
        private float _progress = 0f;
        private Vector3 _savedPosition;
        private GameObject _model => transform.GetChild(0).gameObject;

        private void Start()
        {
            _model.SetActive(false);
        }
        public void Show(Icon icon, Vector3 screenPosition, string text )
        {
            _icon.sprite = icon.GetSprite();
            _label.text = text;
            transform.position = screenPosition;
            _savedPosition = screenPosition;
            _progress = 0f;
            _isShowing = true;
            _canvasGroup.alpha = 0f;
            _model.SetActive(true);
        }

        private void Update()
        {
            if (_isShowing)
            {
                _progress = Mathf.MoveTowards(_progress, 1f, Time.deltaTime / _showTime);
                _canvasGroup.alpha = 1f - _progress;

                transform.position = _savedPosition + Vector3.up * _stepPercent * Screen.height;

                if (_progress >= 1f)
                {
                    _model.SetActive(false);
                    _isShowing = false;
                }
            }
        }
    }
}
