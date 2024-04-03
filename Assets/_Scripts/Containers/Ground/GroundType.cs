using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum GroundType : byte { Default, Dirt, Mud, Grass, Stone,Sand,River }
    public static class GroundTypeExtension
    {
        public static EffectType GetMoveEffect(this GroundType type)
        {
            switch (type)
            {
                case GroundType.Mud: return EffectType.MudEffect;
                    case GroundType.Grass: return EffectType.GrassDust;
                case GroundType.Dirt: return EffectType.DirtDust;
                default: return EffectType.Undefined;
            }
        }
        public static float GetEffectMinStepLength(this GroundType type)
        {
            switch (type)
            {
                case GroundType.Dirt:
                case GroundType.Grass: return 0.1f;
                default: return 1f;
            }
        }
    }
}
