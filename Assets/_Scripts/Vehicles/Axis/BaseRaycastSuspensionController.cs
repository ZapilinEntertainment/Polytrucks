using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class BaseRaycastSuspensionController : AxisControllerBase
	{
        [SerializeField] protected RaycastWheel[] _wheels;
        [SerializeField] protected WheelConfiguration _wheelSettings;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected Transform _centerOfMass;
        private bool _physicsDelay = false;
        protected int _castMask;
        protected float _startMass = 1f, _carSpeed = 0f, _speedPc = 0f;
        protected ColliderListSystem _colliderListSystem;
        public override bool IsActive => base.IsActive & !_physicsDelay;
        override public float Speed => _carSpeed;

        public override Vector3 Forward => _rigidbody.transform.forward;
        public override Vector3 Position => _rigidbody.position;
        public override Quaternion Rotation => _rigidbody.rotation;

        private void Awake()
        {
            _castMask = GameConstants.GetCustomLayermask(CustomLayermask.Tyres);
            if (_centerOfMass != null) _rigidbody.centerOfMass = _centerOfMass.localPosition;
            _startMass = _rigidbody.mass;
        }

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem)
        {
            _colliderListSystem= colliderListSystem;
        }

        virtual protected void FixedUpdate()
        {
            if (!IsActive)
            {
                if (_physicsDelay)
                {
                    _physicsDelay = false;
                }
                return;
            }
            float suspensionLength = _wheelSettings.SuspensionLength,
                springStrength = _wheelSettings.SpringStrength, springDamper = _wheelSettings.SpringDamper,
                passability = AxisController.Passability;
            _carSpeed = Vector3.Dot(Forward, _rigidbody.velocity);
            _speedPc = Mathf.Clamp01(_carSpeed / GameConstants.MAX_SPEED);
            bool isBraking = AxisController.IsBraking;

            foreach (var wheel in _wheels)
            {
                Vector3 pos = wheel.SuspensionPoint.position, up = wheel.SuspensionPoint.up;
                float scaleCf = wheel.ScaleCf;

                if (Physics.Raycast(pos, -up, maxDistance: suspensionLength * scaleCf, hitInfo: out var hit, layerMask: _castMask))
                {
                    float hitDistance = hit.distance;
                    float groundResistance;
                    Vector3 tireVelocity = _rigidbody.GetPointVelocity(pos);

                    if (_colliderListSystem.TryGetGroundInfoCollider(hit.colliderInstanceID, out var collider))
                    {
                        var castInfo = collider.OnWheelCollision(new WheelCollisionInfo(hit.point, tireVelocity, wheel.WheelRadius));
                        hitDistance += castInfo.AdditionalDepth;
                        groundResistance = castInfo.Resistance;
                    }
                    else groundResistance = 0f;

                    

                    float offset = (suspensionLength * scaleCf - hitDistance) / (suspensionLength * scaleCf);
                    float suspensionVelocity = Vector3.Dot(up, tireVelocity);

                    Vector3 steeringForce = CalculateSteerForce(wheel, tireVelocity);
                    Vector3 accelForce;
                    if (isBraking && wheel.HasBrakes)
                    {
                        float brakeForce = _wheelSettings.BrakeForce, tireVel = tireVelocity.magnitude;
                        accelForce = (tireVel > brakeForce ? brakeForce : tireVel) * -tireVelocity;
                    }
                    else
                    {
                        accelForce = wheel.IsMotor ? CalculateAccelerationForce(wheel) : Vector3.zero;
                    }

                    float suspensionForce = (offset * springStrength) - (suspensionVelocity * springDamper);
                    Vector3 moveVector;
                    if (groundResistance == 0f)
                    {
                        moveVector = accelForce;
                    }
                    else
                    {
                        float passabilityCf = passability + (1f - passability) * groundResistance;
                        moveVector = accelForce * passabilityCf - tireVelocity * groundResistance * 0.95f;
                    }
                    _rigidbody.AddForceAtPosition(suspensionForce * up + steeringForce + moveVector, pos);

                    wheel.SetSuspensionCurrentLength(hitDistance);

                }
                else
                {
                    wheel.SetSuspensionCurrentLength(suspensionLength * scaleCf);                    
                }
            }
        }
        private void Update()
        {
            foreach (var wheel in _wheels)
            {
                wheel.Spin(_carSpeed / wheel.WheelRadius * 20f * Time.deltaTime / wheel.ScaleCf );
            }
        }

        protected virtual Vector3 CalculateSteerForce(RaycastWheel wheel, Vector3 tireVelocity)
        {
            Vector3 steeringDir = wheel.SuspensionPoint.right;
            float steeringVel = Vector3.Dot(steeringDir, tireVelocity);
            float desiredVelChange = -steeringVel * _wheelSettings.TireGripCurve.Evaluate(_speedPc);
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
            Vector3 steeringForce = _wheelSettings.TireMass * desiredAccel * steeringDir;
            return steeringForce;
        }
        protected virtual Vector3 CalculateAccelerationForce(RaycastWheel wheel) => Vector3.zero;

        public override void Stabilize()
        {

        }
        public override void Teleport(VirtualPoint point, System.Action onTeleportComplete)
        {
            _physicsDelay = true;
            _rigidbody.MovePosition(point.Position + Vector3.up * _wheelSettings.SuspensionLength * 0.1f);
            _rigidbody.MoveRotation(point.Rotation);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity= Vector3.zero;
            _rigidbody.ResetInertiaTensor();

            if (onTeleportComplete != null) StartCoroutine(PhysicsDelayCoroutine(onTeleportComplete));
        }
        private IEnumerator PhysicsDelayCoroutine(System.Action callback)
        {
            yield return new WaitUntil(() => _physicsDelay == false);
            callback.Invoke();
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                if (_wheels != null && _wheels.Length > 0)
                {
                    bool haveSettings = _wheelSettings != null;
                    foreach (var wheel in _wheels)
                    {
                        Vector3 pos = wheel.SuspensionPoint?.position ?? Vector3.zero;
                        Gizmos.DrawSphere(pos, 0.1f);
                        float size = wheel.WheelRadius;
                        Gizmos.DrawWireSphere(pos + size * Vector3.down, size);
                        if (haveSettings) Gizmos.DrawLine(pos, pos + _wheelSettings.SuspensionLength * wheel.ScaleCf * Vector3.down);
                    }
                }
            }
        }
#endif
    }
}
