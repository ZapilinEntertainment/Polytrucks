using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [RequireComponent(typeof(Truck))]
	public sealed class TEST_AccelerationMeter : MonoBehaviour, IVehicleController
	{
        [SerializeField] private float _speedPc = 0f, _speedValue = 0f, _targetPathLength = 100f;
		private Truck _truck;
        private float _meteringStartTime = 0f;
        private Vector3 _targetPoint;
       // private const float MIN_ACCELERATION_TIME = 1f;

        public void OnItemCompositionChanged() { }
        public void OnItemSold(SellOperationContainer info) { }

        private void Awake()
        {
            _truck = GetComponent<Truck>();
            _truck.AssignVehicleController(this);
            _targetPoint = _truck.Position + _targetPathLength * Vector3.forward;
        }
        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _truck.Gas();
            _meteringStartTime = Time.time;
            do
            {
                _speedPc = _truck.SpeedPc;
                _speedValue = _truck.Speed;
                yield return null;
            }
            while (Vector3.Dot(_truck.Position - _targetPoint, Vector3.forward) < 0);
            Debug.Log(_truck.TruckConfig.TruckID.ToString() + " time is " +  ( Time.time - _meteringStartTime).ToString());
            _truck.ReleaseGas();
        }
    }
}
