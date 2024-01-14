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
		private float _numbersProgress = 1f, _coinProgress = 1f;
		private Color _cachedCoinColor;
		private IAccountDataAgent _dataReader;

		[Inject]
		public void Inject(IAccountDataAgent accountReader)
		{
			_dataReader = accountReader;
		}

        private void Start()
        {
			var playerData = _dataReader.PlayerDataAgent;
			playerData.SubscribeToMoneyChange(OnMoneyCollected);
			_cachedCoinColor = _coinImage.color;
			UpdateMoneyValue(playerData.Money);
        }
		private void UpdateMoneyValue(int money)
		{
			_showingValue = money;
			_moneyLabel.text = _showingValue.ToString();
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
			if (_numbersProgress != 1f)
			{
				_numbersProgress = Mathf.MoveTowards(_numbersProgress, 1f, Time.deltaTime / _effectTime);
				UpdateMoneyValue((int)Mathf.Lerp(_startShowingValue, _targetShowingValue, _numbersProgress));				
			}
			if (_coinProgress != 1f)
			{
                _coinProgress = Mathf.MoveTowards(_coinProgress, 1f, Time.deltaTime / _effectTime);
                float coinEffectVal = Mathf.Abs(_coinProgress);
                _coinImage.rectTransform.localRotation = Quaternion.Euler(0f, coinEffectVal * 360f, 0f);
                _coinImage.color = Color.Lerp(_cachedCoinColor, Color.white, 1f - Mathf.Abs(coinEffectVal - 0.5f) * 2f);
            }
		}
	}
}
