using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/UIElementsPack")]
    public sealed class UIElementsPack : ScriptableObject
	{
		[field: SerializeField] public UIManager GameUiManager { get; private set; }
		[field: SerializeField] public MoneyEffectLabel MoneyEffectLabel { get; private set; }
	}
}
