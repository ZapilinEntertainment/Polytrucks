using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class MoveTutorialQuest : TutorialQuest, IInitializable
    {
        private bool _inputDataIsCorrect = false;
        private DestinationPointsList _points;
        public override bool UseMarkerTracking => throw new System.NotImplementedException();

        [Inject]
        public void Inject(IQuestDataContainer questDataContainer, InitializableManager initManager)
        {
            _points = questDataContainer as DestinationPointsList;
            if (_points == null) Debug.LogError("move tutorial quest not started - quest data container is not DestinationPointsList");
            else
            {
                _inputDataIsCorrect = true;
            }
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }


        public override bool CheckConditions()
        {
            throw new System.NotImplementedException();
        }

        public override IQuestMessage FormNameMsg()
        {
            throw new System.NotImplementedException();
        }

        public override IQuestMessage FormProgressionMsg()
        {
            throw new System.NotImplementedException();
        }

        public override Vector3 GetWorldPosition()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
