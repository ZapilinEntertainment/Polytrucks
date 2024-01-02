using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class QuestDataInstaller : MonoInstaller
	{
		[SerializeField] private MonoBehaviour _questDataContainer;

        public override void InstallBindings()
        {
            if (_questDataContainer is IQuestDataContainer) Container.Bind<IQuestDataContainer>().FromInstance(_questDataContainer as IQuestDataContainer);
            else Debug.LogError("quest data container is not suitable");
        }
    }
}
