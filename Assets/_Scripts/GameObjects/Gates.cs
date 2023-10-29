using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class Gates : MonoBehaviour
	{
		[SerializeField] private bool _isOpened = false;
        [SerializeField] private float _openTime = 1f, _openStep = 2f;
        [SerializeField] private Transform _leftDoor, _rightDoor;
		private bool _isTweening = false;
		
		public void Open()
		{
			if (!_isOpened)
			{
				if (_isTweening)
				{
					DOTween.Kill(_leftDoor.transform);
					DOTween.Kill(_rightDoor.transform);
				}
				else _isTweening = true;
				_leftDoor.DOLocalMoveX(-_openStep, _openTime, true);
				_rightDoor.DOLocalMoveX(_openStep, _openTime, true).OnComplete(OnDoorOpened);
			}
		}
		private void OnDoorOpened()
		{
			_isOpened = true;
			_leftDoor.gameObject.SetActive(false);
			_rightDoor.gameObject.SetActive(false);
			_isTweening = false;
		}
	}
}