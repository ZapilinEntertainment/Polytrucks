using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class GroundCollider : MonoBehaviour
	{
        [SerializeField] protected GroundType _groundType = GroundType.Default;
        [SerializeField] protected Collider _collider;
        [SerializeField] protected GroundPassabilitySettings _passabilityParameters;
        private int _colliderID = -1;
        private EffectType _effectType = EffectType.Undefined;
        private ColliderListSystem _colliderListSystem;
        private EffectsService _effectsService;
        public int GetColliderID() => _colliderID;

        [Inject]
        public void Inject(ColliderListSystem colliderListSystem, EffectsService effectsService)
        {
            _colliderListSystem = colliderListSystem;
            _effectsService = effectsService;
        }

        virtual protected void Start() 
        { 
            _effectType = _groundType.GetMoveEffect();
            _colliderID = _collider.GetInstanceID();

            _colliderListSystem.AddGroundInfo(this);
        }
        virtual public GroundCastInfo OnWheelCollision(WheelCollisionInfo wheelStep)
        {
            if (_effectType != EffectType.Undefined && wheelStep.StepSqrLength > 1f) _effectsService.EmitEffect(_effectType, wheelStep.Pos, -wheelStep.Forward);
            return FormCastInfo(wheelStep);
        }
        
        virtual protected GroundCastInfo FormCastInfo(WheelCollisionInfo info) => new GroundCastInfo(_passabilityParameters.Harshness, _passabilityParameters.Resistance, 0f);
    }
}
