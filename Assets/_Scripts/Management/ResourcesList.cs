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
    }

    public sealed class ResourcesInstaller : Installer<ResourcesInstaller>
    {
        private ResourcesList _resourcesManager;
        public ResourcesInstaller(ResourcesList resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }
        public override void InstallBindings()
        {
            Container.Bind<ObjectsPack>().FromScriptableObject(_resourcesManager.ObjectsPack).AsCached();
            Container.Bind<IconsPack>().FromScriptableObject(_resourcesManager.IconsPack).AsCached();
            Container.Bind<CratesPack>().FromScriptableObject(_resourcesManager.CratesPack).AsCached();

            Container.Bind<UIColorsPack>().FromScriptableObject(_resourcesManager.ColorsPack).AsCached();
            Container.Bind<UIElementsPack>().FromScriptableObject(_resourcesManager.UiElementsPack).AsCached();
            Container.Bind<EconomicSettings>().FromScriptableObject(_resourcesManager.EconomicSettings).AsCached();
        }
    }
}
