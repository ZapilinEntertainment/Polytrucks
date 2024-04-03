using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class IntegrityModule : IVehicleHitEventSubscriber, ITrackableVehicleModule, IDisposable
	{
        private readonly IIntegrityConfiguration _config;
        public bool IsDestructed { get; private set; } = false;
		public float HP { get; private set; }
		public float MeaningValue => HP / _config.MaxHP;
		public Action OnDestructedEvent;
		public Action OnModuleDisposedEvent { get; set; }


		
		public IntegrityModule(CollidersHandler collidersHandler, IIntegrityConfiguration config, float healthPc = 1f)
		{
			_config = config;
			HP = _config.MaxHP * healthPc;
			collidersHandler.SubscribeToHits(this);
		}

        public void OnVehicleHit(CollisionResult result)
        {
            if (!IsDestructed )
			{
				float damage = result.Impulse * _config.HitIncomeDamageCf;
				ApplyDamage(damage);
			}
        }
		public void Update(float t)
		{
			if (!IsDestructed)
			{
				ApplyDamage(_config.HpDegradeSpeed * t);
			}
		}
		private void ApplyDamage(float x)
		{
			HP -= x;
			if (HP < 0f) {
				HP = 0f;
				IsDestructed = true;
				OnDestructedEvent?.Invoke();
			}
		}

		public void Repairs(float percent)
		{
			int maxHP = _config.MaxHP;
			HP = Mathf.Clamp(HP + percent * maxHP, 0f, maxHP);
		}

        public void Dispose()
        {
			OnModuleDisposedEvent?.Invoke();
        }
    }
}
