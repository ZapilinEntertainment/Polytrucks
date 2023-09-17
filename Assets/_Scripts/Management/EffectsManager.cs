using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

    public enum EffectType : byte { Undefined}
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
    public sealed class EffectsManager : MonoBehaviour
	{       
        [SerializeField] private VolumeScalableEmitter _someEmitter;
        private Dictionary<EffectType, VolumeScalableEmitter> _emitters = new Dictionary<EffectType, VolumeScalableEmitter>();

        public void PlayEffect(EffectType effectType, Vector3 pos, Vector3 dir, float scale = VolumeScalableEmitter.DEFAULT_SCALE)
        {
            if (effectType == EffectType.Undefined) return;

            VolumeScalableEmitter emitter;
            if (!_emitters.TryGetValue(effectType, out emitter))
            {
                // load emitter
                emitter = null;
            }
            emitter.Play(pos, dir, scale);
        }
    }
}
