using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class CountTracker : MonoBehaviour, ICountTracker
	{
		[SerializeField] private TMPro.TMP_Text _label;
        private int _maxCount = 1;

        public void Setup(int maxCount, int currentCount, bool isActive)
        {
            _maxCount= maxCount;
            OnCountChanged(currentCount);
            OnTrackStatusChanged(isActive);
        }
        public void OnCountChanged(int x)
        {
            _label.text = x.ToString() + '/' + _maxCount.ToString();
        }

        public void OnTrackStatusChanged(bool x)
        {
            gameObject.SetActive(x);
        }

        public void StopTracking()
        {
           Destroy(gameObject);
        }
    }
}
