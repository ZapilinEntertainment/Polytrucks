using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public abstract class TutorialQuest : QuestBase
    {
        public override QuestType QuestType => QuestType.Tutorial;
    }
}
