using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class ActivableRotator : Rotator, IActivableMechanism
	{
		[SerializeField] private bool _isActive = false;
        public bool IsActive => _isActive;
        public Action OnActivatedEvent { get; set; }

        public void Activate()
        {
            if (!IsActive)
            {
                _isActive = true;
                OnActivatedEvent?.Invoke();
            }
        }
        protected override void Update()
        {
            if (_isActive) base.Update();
        }
    }
}
