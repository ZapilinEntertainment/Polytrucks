using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum GroundType : byte { Default, Dirt, Mud, Grass, Stone,Sand,River }
    public static class GroundTypeExtension
    {
        public static EffectType GetMoveEffect(this GroundType type)
        {
            if (type == GroundType.Mud) return EffectType.MudEffect;
            else return EffectType.Undefined;
        }
    }
}
