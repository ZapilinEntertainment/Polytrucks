using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    
    public sealed class ResourcesManager : MonoBehaviour
    {
        [SerializeField] private EffectsManager _effectsManager;
        private ResourceLoader _resourceLoader = new ResourceLoader();

        public void PlayEffect(EffectType effectType, Vector3 pos, Vector3 dir, float scale = VolumeScalableEmitter.DEFAULT_SCALE) => _effectsManager.PlayEffect(effectType, pos, dir, scale);
        public void PlayEffect(EffectType effectType, Vector3 pos, float scale = VolumeScalableEmitter.DEFAULT_SCALE) => PlayEffect(effectType, pos, Vector3.up, scale); 
    }
}
