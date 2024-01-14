using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {    
    [System.Serializable]
    public class VolumeScalableEmitter
    {
        public ParticleSystem _system;
        public int _baseCount;
        public const float DEFAULT_SCALE = 0f;

        public void Play(Vector3 pos, float scale = 0f)
        {
            _system.transform.position = pos;
            if (scale != 0f) _system.Emit((int)(_baseCount * scale));
            _system.Play(true);
        }
        public void Play(Vector3 pos, Vector3 normal, float scale = DEFAULT_SCALE)
        {
            _system.transform.forward = normal;
            Play(pos, scale);
        }
    }
    public sealed class EffectsManager
	{
        private class EffectEmitter
        {
            private ParticleSystem _particleSystem;
            private Transform _transform;

            public EffectEmitter(ParticleSystem particleSystemPrefab)
            {
                _particleSystem = UnityEngine.Object.Instantiate(particleSystemPrefab);
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
        private EffectEmitter GetEmitter(EffectType effectType)
        {
            EffectEmitter emitter;
            if (!_emitters.TryGetValue(effectType, out emitter))
            {
                var prefab = _effectsPack.GetEffectPrefab(effectType);
                if (prefab != null) emitter = new EffectEmitter(prefab);
            }
            return emitter;
        }
    }
}
