using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class AnimatedAppearMechanismEffect : MonoBehaviour
	{
        [SerializeField] private float _effectTime = 1f;
		[SerializeField] private SpriteRenderer[] _renderers;
		[SerializeField] private MonoBehaviour _activableMechanism;
        [SerializeField] private GameObject _modelObject;
        private bool _isActivated = false;

        private void Start()
        {
            var mechanism = _activableMechanism as IActivableMechanism;
            _isActivated = mechanism.IsActive;
            _modelObject.SetActive(_isActivated);
            if (!_isActivated)
            {
                foreach (var renderer in _renderers)
                {
                    var color = renderer.color;
                    color.a = 0f;
                    renderer.color = color;
                }
                mechanism.OnActivatedEvent += OnMechanismEnabled;
            }
            else Destroy(this);
        }

        private void OnMechanismEnabled()
        {
            if (!_isActivated)
            {
                _isActivated = true;
                _modelObject.SetActive(true);
                foreach (var renderer in _renderers)
                {
                    var color = renderer.color;
                    color.a = 1f;
                    renderer.DOColor(color, _effectTime);
                }
                Destroy(this);
            }
        }        
    }
}
