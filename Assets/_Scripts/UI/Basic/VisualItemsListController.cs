using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

	public class VisualItemsListController : MonoBehaviour
	{
		[SerializeField] protected ItemsVisualSelectionConfig _visualConfig;
		[SerializeField] protected ItemButtonHandler[] _itemButtons;
		protected int _selectedIndex = -1;
		protected ItemButtonHandler _selectedButton;
		public Action<int> OnItemSelectedEvent;

		public void Setup(int selectedIndex, IReadOnlyList<Sprite> sprites)
		{
			int existItemsCount = sprites.Count;
			for (int i = 0; i< _itemButtons.Length; i++)
			{
				if (i < existItemsCount)
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
