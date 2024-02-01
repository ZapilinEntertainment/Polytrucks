using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/GroundSettings")]
    public class GroundSettings : ScriptableObject
	{
		[field: SerializeField] public float Harshness { get; private set; } = 0.25f;
		[SerializeField] private float _minDepth = 0.01f, _maxDepth = 0.1f;
		[SerializeField] private AnimationCurve _zCurve, _xCurve;

		public GroundCastInfo GetCastInfo(float x, float z) => new GroundCastInfo(Harshness, Mathf.Lerp(_minDepth, _maxDepth, GetDepthValue(x,z)));

		private float GetDepthValue(float x, float z)
		{
			return (_zCurve.Evaluate(z) + _xCurve.Evaluate(x)) / 2f;
		}
	}
}
