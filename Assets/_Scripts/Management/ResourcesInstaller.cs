using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks
{
    
    public sealed class ResourcesInstaller : MonoInstaller
    {
        [field: SerializeField] public CratesPack CratesPack { get; private set; }
        [field:SerializeField] public ObjectsPack ObjectsPack { get; private set; }
        [field:SerializeField] public IconsPack IconsPack { get; private set; }        
        [field: SerializeField] public UIColorsPack ColorsPack { get; private set; }
        [field: SerializeField] public UIElementsPack UiElementsPack { get; private set; }
        [field: SerializeField] public EconomicSettings EconomicSettings { get; private set; }
        [field: SerializeField] public EffectsPack EffectsPack { get; private set; }

        public override void InstallBindings()
        {            
            Container.Bind<ResourcesInstaller>().FromInstance(this).AsSingle();

            Container.Bind<ObjectsPack>().FromScriptableObject(ObjectsPack).AsCached();
            Container.Bind<IconsPack>().FromScriptableObject(IconsPack).AsCached();
            Container.Bind<CratesPack>().FromScriptableObject(CratesPack).AsCached();
            Container.Bind<EffectsPack>().FromScriptableObject(EffectsPack).AsCached();

            Container.Bind<UIColorsPack>().FromScriptableObject(ColorsPack).AsCached();
            Container.Bind<UIElementsPack>().FromScriptableObject(UiElementsPack).AsCached();
            Container.Bind<EconomicSettings>().FromScriptableObject(EconomicSettings).AsCached();
            Debug.Log("install resources");
        }
    }
}
