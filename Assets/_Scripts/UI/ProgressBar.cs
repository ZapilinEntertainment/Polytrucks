using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Polytrucks
{
    public sealed class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private TMPro.TMP_Text _label;
        private bool _tracking = false;
        private IProgressionObject _target;

        private void Start() => StopTracking();
        public void TrackObject(IProgressionObject i_target)
        {
            if (i_target?.IsProgressing ?? false)
            {                
                _target = i_target;
                transform.GetChild(0).gameObject.SetActive(true);
                _tracking = true;
            }
        }

        private void Update()
        {
            if (_tracking)
            {
                if (_target?.IsProgressing ?? false)
                {
                    transform.position = Camera.main.WorldToScreenPoint(_target.IndicatorPosition);
                    _progressBar.fillAmount = _target.Progress;
                }
                else StopTracking();
            }
        }

        public void StopTracking(IProgressionObject obj)
        {
            if (_tracking && _target == obj) StopTracking();
        }
        private void StopTracking()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            _tracking = false;
            _target = null;
        }
    }
}
