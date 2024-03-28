using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class ExhaustVehicleEffect : MonoBehaviour
	{
		[SerializeField] private Vehicle _vehicle;
		[SerializeField] private ParticleSystem _effect;
		[SerializeField] private int _minParticlesCount = 10, _maxParticlesCount = 30;

		private void Update()
		{
			_effect.Emit((int)Mathf.Lerp(_minParticlesCount, _maxParticlesCount, _vehicle.GasValue));
		}
	}
}
