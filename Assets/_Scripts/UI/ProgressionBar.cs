using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZE.Polytrucks {
	public class ProgressionBar : MonoBehaviour
	{
		[SerializeField] private GameObject _divisionPrefab;
		[SerializeField] private Image _progressionBar;
		[SerializeField] private TMPro.TMP_Text _progressLabel;

		public void Setup(int divisions, float startPercent = 0f)
		{
			SetDivisions(divisions);
			SetProgress(startPercent);
		}

		private void SetDivisions(int x)
		{
			Transform host = _divisionPrefab.transform.parent;
			int existDivisions = host.childCount;
			if (existDivisions < x)
			{
				for (int i = existDivisions; i < x;i++)
				{
					Instantiate(_divisionPrefab, host); 
				}
				existDivisions = x;
			}
			for (int i = 0; i < existDivisions; i++)
			{
				host.GetChild(i).gameObject.SetActive(i < x);
			}
		}
		public void SetProgress(int current, int total)
		{
			_progressLabel.text = $"{current}/{total}";
			SetProgress(current / (float) total);
		} 
		public void SetProgress(float percent) => _progressionBar.fillAmount= percent;
	}
}
