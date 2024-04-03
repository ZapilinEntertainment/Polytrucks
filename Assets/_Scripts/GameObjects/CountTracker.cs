using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class CountTracker : MonoBehaviour, ICountTracker
	{
		[SerializeField] private TMPro.TMP_Text _label;
        private bool _isActive = false; 
        private int _maxCount = 1;
        private ICountTrackable _trackableObject;

        public void SetTrackingObject(ICountTrackable trackable)
        {
            _trackableObject = trackable;
            _trackableObject.Subscribe(this);

            _maxCount = _trackableObject.TargetCount;
            OnCountChanged(_trackableObject.CollectedCount);
            _isActive = true;
            gameObject.SetActive(true);
        }
        public void OnCountChanged(int x)
        {
            _label.text = x.ToString() + '/' + _maxCount.ToString();
        }

        public void OnTrackableDisposed()
        {
           Destroy(gameObject);
        }
    }
}
