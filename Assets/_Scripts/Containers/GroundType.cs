using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum GroundType : byte { Default, Dirt }
    public static class GroundTypeExtension
    {
        public static EffectType GetMoveEffect(this GroundType type)
        {
            if (type == GroundType.Dirt) return EffectType.DirtEffect;
            else return EffectType.Undefined;
        }
    }
}
