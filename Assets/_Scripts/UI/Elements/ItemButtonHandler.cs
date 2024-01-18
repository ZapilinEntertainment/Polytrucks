using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ZE.Polytrucks {
	public sealed class ItemButtonHandler : MonoBehaviour
	{
		[SerializeField] private Image _icon, _background;
		[SerializeField] private GameObject _selectionFrame;
		private ItemsVisualSelectionConfig _visualConfig;
		private Action _clickAction;

		public void Setup(Sprite icon, ItemsVisualSelectionConfig config, bool isSelected, Action clickAction)
		{
			_clickAction= clickAction;
			_visualConfig= config;
			_icon.sprite = icon;
			SetSelection(isSelected);
			SetActivity(true);
		}

		public void SetSelection(bool x)
		{
			if (x)
			{
				_icon.color = _visualConfig.IconColors.SelectedColor;
				_background.color = _visualConfig.BackgroundColors.SelectedColor;
				_selectionFrame.SetActive(true);
			}
			else
			{
				_icon.color = _visualConfig.IconColors.NormalColor;
				_background.color = _visualConfig.IconColors.NormalColor;
				_selectionFrame.SetActive(false);
			}
		}

		public void BUTTON_Click() => _clickAction?.Invoke();
		public void SetActivity(bool x) => gameObject.SetActive(x);
	}
}
