using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks
{
    
    public sealed class ResourcesList : MonoBehaviour
    {
        [field: SerializeField] public CratesPack CratesPack { get; private set; }
        [field:SerializeField] public ObjectsPack ObjectsPack { get; private set; }
        [field:SerializeField] public IconsPack IconsPack { get; private set; }        
        [field: SerializeField] public UIColorsPack ColorsPack { get; private set; }
        [field: SerializeField] public UIElementsPack UiElementsPack { get; private set; }
        [field: SerializeField] public EconomicSettings EconomicSettings { get; private set; }
        [field: SerializeField] public EffectsPack EffectsPack { get; private set; }

        public void BindToContainer(DiContainer container)
        {
            container.Bind<ObjectsPack>().FromScriptableObject(ObjectsPack).AsCached();
            container.Bind<IconsPack>().FromScriptableObject(IconsPack).AsCached();
            container.Bind<CratesPack>().FromScriptableObject(CratesPack).AsCached();
            container.Bind<EffectsPack>().FromScriptableObject(EffectsPack).AsCached();

            container.Bind<UIColorsPack>().FromScriptableObject(ColorsPack).AsCached();
            container.Bind<UIElementsPack>().FromScriptableObject(UiElementsPack).AsCached();
            container.Bind<EconomicSettings>().FromScriptableObject(EconomicSettings).AsCached();
        }
    }
}
