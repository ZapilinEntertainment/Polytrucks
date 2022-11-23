using System;
using UnityEditor;
using UnityEngine;

namespace Polytrucks
{
    public class WheelPlatform : Car
    {
        [SerializeField] private AnimationCurve _resistCurve;
        protected override float GetSuspensionAmount(float distance) => _resistCurve.Evaluate( Round(1f - distance / SuspensionLength, 2));
    }
}