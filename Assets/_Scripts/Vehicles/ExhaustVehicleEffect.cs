using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class ExhaustVehicleEffect : MonoBehaviour
	{
		[SerializeField] private Vehicle _vehicle;
		[SerializeField] private ParticleSystem _effect;

        private void Start()
        {
            if (_vehicle.VehicleController != null) _effect.Play();
            _vehicle.OnVehicleControllerChangedEvent+= OnVehicleControllerChanged;
        }
        private void OnVehicleControllerChanged(IVehicleController controller)
        {
            if (controller != null)
            {
                if (!_effect.isPlaying) _effect.Play();
            }
            else
            {
                if (_effect.isPlaying) _effect.Stop();
            }
        }
    }
}
