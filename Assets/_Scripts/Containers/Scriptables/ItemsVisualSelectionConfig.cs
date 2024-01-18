using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/UI/ItemsVisualSelectionConfig")]
	public sealed class ItemsVisualSelectionConfig : MonoBehaviour
	{
		[System.Serializable]
		public class ItemColors
		{
			public Color NormalColor = Color.white;
			public Color SelectedColor = Color.yellow;
			public Color DisabledColor = Color.gray;
		}
		[field:SerializeField] public ItemColors BackgroundColors { get; private set; }
		[field:SerializeField] public ItemColors IconColors { get; private set; }
	}
}
