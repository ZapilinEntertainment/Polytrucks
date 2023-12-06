using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private MoneyEffectLabel _moneyEffectPrefab;
        [SerializeField] private ChoicePopup _choicePopupPrefab;
        public override void InstallBindings()
        {            
            // all bindings will work only inside ui game object context;

            Container.BindMemoryPool<MoneyEffectLabel, MoneyEffectLabel.Pool>()
            .WithInitialSize(8)
            .FromComponentInNewPrefab(_moneyEffectPrefab).
            UnderTransformGroup("uiEffects");
            Container.Bind<ChoicePopup>().FromComponentInNewPrefab(_choicePopupPrefab).UnderTransform(_uiManager.PopupHost).AsCached();

            Debug.Log("ui install");
        }
    }
}
