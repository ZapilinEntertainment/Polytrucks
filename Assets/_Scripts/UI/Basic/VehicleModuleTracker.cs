using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZE.Polytrucks {
	public class VehicleModuleTracker<T> : MonoBehaviour
	{
        protected enum TrackerStatus : byte
        {
            Disabled,Appearing, Active,Disabling
        }

        [SerializeField] private float _appearTime = 0.5f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _content;
        protected TrackerStatus _trackerStatus { get; private set; }

        protected void ChangeStatus(TrackerStatus status, bool forced = false)
        {
            if (_trackerStatus != status || forced)
            {                
                switch (status)
                {
                    case TrackerStatus.Disabled:
                        {
                            _content.SetActive(false);
                            _canvasGroup.DOKill();
                            _canvasGroup.alpha = 0f;                            
                            break;
                        }
                    case TrackerStatus.Appearing:
                        {
                            if (_trackerStatus == TrackerStatus.Active) return;
                            _content.SetActive(true);
                            _canvasGroup.DOFade(1f, _appearTime);
                            break;
                        }
                    case TrackerStatus.Active:
                        {
                            _content.SetActive(true);
                            _canvasGroup.DOKill();
                            _canvasGroup.alpha = 1f;
                            break;
                        }
                    case TrackerStatus.Disabling:
                        {
                            if (_trackerStatus == TrackerStatus.Disabled) return;
                            _content.SetActive(true);
                            _canvasGroup.DOFade(0f, _appearTime).OnComplete(() => ChangeStatus(TrackerStatus.Disabled));
                            break;
                        }
                }
                _trackerStatus = status;
            }
        }
    }
}
