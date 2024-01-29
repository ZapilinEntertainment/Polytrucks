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

		public void Setup(int divisions, int currentValue, int maxValue)
		{
			SetDivisions(divisions);
			SetProgress(currentValue, maxValue);
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
			if (_progressLabel != null) _progressLabel.text = $"{current}/{total}";
			i_SetProgress(current / (float) total);
		} 
		public void SetProgress(float pc)
		{
            if (_progressLabel != null) _progressLabel.text = ((int)(pc * 100f)).ToString();
			i_SetProgress(pc);
        }
		virtual protected void i_SetProgress(float percent) => _progressionBar.fillAmount= percent;
	}
}
