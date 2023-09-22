using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    public interface ILevelSubscriber
    {
        public void OnLevelLoaded(LevelSettings settings);
        public void OnLevelClear();
    }
}
