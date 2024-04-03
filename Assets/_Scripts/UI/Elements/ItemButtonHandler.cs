using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ZE.Polytrucks {
	public sealed class ItemButtonHandler : MonoBehaviour
	{
		[SerializeField] private RectTransform _baseRect;
		[SerializeField] private Image _icon, _background;
		[SerializeField] private GameObject _selectionFrame;
		private VisualItemContainer _item;
		private ItemsVisualSelectionConfig _visualConfig;
		private Action _clickAction;

		public void SetSize(float width, float height)
		{
			_baseRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			_baseRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
		public void FitIntoAnchors(float minX, float maxX, float minY, float maxY)
		{
			_baseRect.anchorMin = new Vector2(minX, minY);
			_baseRect.anchorMax = new Vector2(maxX, maxY);
			_baseRect.offsetMin = Vector2.zero;
			_baseRect.offsetMax = Vector2.zero;
		}
		public void Setup(VisualItemContainer item, ItemsVisualSelectionConfig config, bool isSelected, Action clickAction)
		{
			_item = item;
			_clickAction= clickAction;
			_visualConfig= config;
			_icon.sprite = item.Sprite;
			SetSelection(isSelected);
			SetActivity(true);
		}

		public void SetSelection(bool x)
		{
			if (!_item.IsUnlocked)
			{
				_icon.color = _visualConfig.IconColors.DisabledColor;
				_background.color = _visualConfig.BackgroundColors.DisabledColor;
				_selectionFrame.SetActive(false);
			}
			else
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
		}

		public void BUTTON_Click() => _clickAction?.Invoke();
		public void SetActivity(bool x) => gameObject.SetActive(x);
	}
}
