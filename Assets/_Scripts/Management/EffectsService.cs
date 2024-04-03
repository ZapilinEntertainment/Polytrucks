using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {    
    public sealed class EffectsService
	{
        private class EffectEmitter
        {
            private ParticleSystem _particleSystem;
            private Transform _transform;
            private int _baseCount = 5;

            public EffectEmitter(ParticleSystem particleSystemPrefab)
            {
                _particleSystem = UnityEngine.Object.Instantiate(particleSystemPrefab);
                var emissionModule = _particleSystem.emission;
                if (emissionModule.burstCount > 0)
                {
                    _baseCount = (int)(emissionModule.GetBurst(0).count.constant);
                }                
                _transform = _particleSystem.transform;
            }

            public void Play(Vector3 pos)
            {
                _transform.position = pos;
                _particleSystem.Play();
            }
            public void Play(Vector3 pos, Vector3 dir)
            {
                _transform.forward = dir;
                Play(pos);
            }
            // Emit works better with multiple calls per frame
            public void Emit(Vector3 pos, float percent = 1f)
            {
                _transform.position = pos;
                _particleSystem.Emit((int)(_baseCount * percent));
            }
            public void Emit(Vector3 pos, Vector3 dir, float percent = 1f)
            {
                _transform.forward = dir;
                Emit(pos, percent);
            }
        }

        private EffectsPack _effectsPack;
        private Dictionary<EffectType, EffectEmitter> _emitters = new Dictionary<EffectType, EffectEmitter>();

        [Inject]
        public void Inject(EffectsPack effectsPack)
        {
            _effectsPack = effectsPack;
        }

        public void PlayEffect(EffectType effectType, Vector3 pos)
        {
            if (effectType == EffectType.Undefined) return;            
            GetEmitter(effectType).Play(pos);
        }
        public void PlayEffect(EffectType effectType, Vector3 pos,Vector3 dir)
        {
            if (effectType == EffectType.Undefined) return;
            GetEmitter(effectType).Play(pos, dir);
        }
        public void EmitEffect(EffectType effectType, Vector3 pos, Vector3 dir, float power = 1f)
        {
            if (effectType == EffectType.Undefined) return;
            GetEmitter(effectType).Emit(pos, dir, power);
        }
        private EffectEmitter GetEmitter(EffectType effectType)
        {
            EffectEmitter emitter;
            if (!_emitters.TryGetValue(effectType, out emitter))
            {
                var prefab = _effectsPack.GetEffectPrefab(effectType);
                if (prefab != null)
                {
                    emitter = new EffectEmitter(prefab);
                    _emitters.Add(effectType, emitter);
                }
            }
            return emitter;
        }
    }
}
