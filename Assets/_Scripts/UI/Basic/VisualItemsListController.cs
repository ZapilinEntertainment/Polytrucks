using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

	public class VisualItemsListController : MonoBehaviour
	{
		[SerializeField] protected ItemsVisualSelectionConfig _visualConfig;
		[SerializeField] protected ItemButtonHandler _buttonPrefab;
		[SerializeField] protected RectTransform _buttonsHost, _iconsZone;
		protected ItemButtonHandler[] _itemButtons = new ItemButtonHandler[0];
		protected int _selectedIndex = -1;
		protected ItemButtonHandler _selectedButton;
		public Action<int> OnItemSelectedEvent;

		public void Setup(int selectedIndex, IReadOnlyList<Sprite> sprites)
		{
			int listItemsCount = sprites.Count, buttonsCount = _itemButtons.Length;
			if (buttonsCount < listItemsCount)
			{
				var newButtonsArray = new ItemButtonHandler[listItemsCount];
				if (buttonsCount > 0) _itemButtons.CopyTo(newButtonsArray, 0);
				for (int i = buttonsCount; i < listItemsCount; i++)
				{
					newButtonsArray[i] = Instantiate(_buttonPrefab, _buttonsHost);
				}
				_itemButtons = newButtonsArray;
				buttonsCount = listItemsCount;
				//resizingContent
				float buttonHeight = _iconsZone.rect.height, zoneWidth = _iconsZone.rect.width;
				float totalWidth = listItemsCount* buttonHeight;
				if (zoneWidth < totalWidth)
				{
					buttonHeight = zoneWidth / listItemsCount;
					totalWidth = buttonHeight * listItemsCount;
				}
				_buttonsHost.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
				_buttonsHost.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight);

				const float minY = 0f, maxY = 1f;
				float delta = 1f / buttonsCount;
				for (int i = 0; i < buttonsCount;i++)
				{
					_itemButtons[i].FitIntoAnchors(delta * i, delta  * (i +1), minY, maxY);
				}
            }

			for (int i = 0; i< _itemButtons.Length; i++)
			{
				if (i < listItemsCount)
				{
					int index = i;
					_itemButtons[i].Setup(sprites[i], _visualConfig, selectedIndex == i, () => OnItemSelected(index));
				}
				else
				{
					_itemButtons[i].SetActivity(false);
				}
			}
			_selectedIndex= selectedIndex;
			_selectedButton = _itemButtons[selectedIndex];
		}

		private void OnItemSelected(int x)
		{
			if (_selectedIndex > 0 && x != _selectedIndex)
			{
				_itemButtons[_selectedIndex].SetSelection(false);
			}
			_selectedIndex = x;
			_itemButtons[_selectedIndex].SetSelection(true);
			OnItemSelectedEvent?.Invoke(_selectedIndex);
		}
	}
}
