using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class MoneyTracker : MonoBehaviour
	{
		[SerializeField] private float _effectTime = 1f;
		[SerializeField] private TMPro.TMP_Text _moneyLabel;
		[SerializeField] private Image _coinImage;
		private int _showingValue = 0,_startShowingValue = 0, _targetShowingValue = 0;
		private float _numbersProgress = 0f, _coinProgress = 0f;
		private Color _cachedCoinColor;

		[Inject]
		public void Inject(PlayerData playerData)
		{
			playerData.SubscribeToMoneyChange(OnMoneyCollected);
		}

        private void Start()
        {
			_cachedCoinColor = _coinImage.color;
        }

        private void OnMoneyCollected(int totalMoney)
		{
			if (totalMoney != _showingValue)
			{
				_startShowingValue = _showingValue;
				_targetShowingValue = totalMoney;
				_numbersProgress = 0f;
				if (_coinProgress > 0f) _coinProgress -= 1f;
			}
		}

		private void Update()
		{
			if (_numbersProgress != 0f)
			{
				_numbersProgress = Mathf.MoveTowards(_numbersProgress, 1f, Time.deltaTime / _effectTime);
				_coinProgress = Mathf.MoveTowards(_coinProgress, 1f, Time.deltaTime / _effectTime);
				_showingValue = (int)Mathf.Lerp(_showingValue, _targetShowingValue, _numbersProgress);

				_moneyLabel.text = _showingValue.ToString();
				float coinEffectVal = Mathf.Abs(_coinProgress);
                _coinImage.rectTransform.localRotation = Quaternion.Euler(0f, coinEffectVal * 360f, 0f);
				_coinImage.color = Color.Lerp(_cachedCoinColor, Color.white, 1f - Mathf.Abs(coinEffectVal - 0.5f) * 2f);
			}
		}
	}
}
