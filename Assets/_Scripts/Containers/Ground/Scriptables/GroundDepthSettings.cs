using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/GroundDepthSettings")]
    public class GroundDepthSettings : ScriptableObject
	{
        [SerializeField] private float _minDepth = 0.01f, _maxDepth = 0.1f;
        [SerializeField] private AnimationCurve _zCurve, _xCurve;

        public float GetDepth(Vector2 internalCoords) => Mathf.Lerp(_minDepth, _maxDepth, GetNormalizedDepth(internalCoords.x, internalCoords.y));
        private float GetNormalizedDepth(float x, float z)
        {
            return (_zCurve.Evaluate(z) + _xCurve.Evaluate(x)) / 2f;
        }
    }
}
