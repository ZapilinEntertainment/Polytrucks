using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	

    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/GroundSettings")]
    public class GroundSettings : ScriptableObject
	{
		[field: SerializeField] public bool UseVisualDeformation { get; private set; }
		[field: SerializeField] public GroundType GroundType { get; private set; } = GroundType.Default;
		[field: SerializeField] public float Harshness { get; private set; } = 0.25f;
		[field: SerializeField] public float Fluidity { get; private set; } = 1f;
		[SerializeField] private float _minDepth = 0.01f, _maxDepth = 0.1f, _visualHeightCf = 2f;
		[SerializeField] private AnimationCurve _zCurve, _xCurve;
		public float HeightDelta => (_maxDepth - _minDepth) * _visualHeightCf;

		public GroundCastInfo GetCastInfo(Vector2 internalCoords) => new GroundCastInfo(Harshness, Mathf.Lerp(_minDepth, _maxDepth, GetDepthValue(internalCoords.x, internalCoords.y)));

		private float GetDepthValue(float x, float z)
		{
			return (_zCurve.Evaluate(z) + _xCurve.Evaluate(x)) / 2f;
		}
	}
}
