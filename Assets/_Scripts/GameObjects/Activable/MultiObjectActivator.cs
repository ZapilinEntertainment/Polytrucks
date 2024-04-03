using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public enum ActivateMethod : byte
    {
        AsGameObject, AsParticleSystem
    }
    [Serializable]
    public struct ActivableObjectContainer
    {
        public GameObject Object;
        public ActivateMethod ActivateMethod;

        public void Activate()
        {
            switch (ActivateMethod)
            {
                case ActivateMethod.AsParticleSystem:
                    {
                        if (Object.TryGetComponent<ParticleSystem>(out var ps)) ps.Play(); 
                        break;
                    }
                default: Object.SetActive(true); break;
            }
        }
    }
	public sealed class MultiObjectActivator : MonoBehaviour, IActivableMechanism
	{
		[SerializeField] private ActivableObjectContainer[] _activableObjects;
        [SerializeField] private MonoBehaviour[] _activableScripts;
        public bool IsActive { get; private set; }

        public Action OnActivatedEvent { get ; set; }

        public void Activate()
        {
            if (!IsActive)
            {
                if (_activableObjects.Length > 0)
                {
                    foreach (var obj in _activableObjects)
                    {
                        obj.Activate();
                    }
                }
                if (_activableScripts.Length > 0)
                {
                    foreach (var scr in _activableScripts)
                    {
                        (scr as IActivableMechanism)?.Activate();
                    }
                }
                IsActive = true;
                OnActivatedEvent?.Invoke();
            }
        }
    }
}
