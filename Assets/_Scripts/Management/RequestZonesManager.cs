using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class RequestZonesManager
	{
		private SignalBus _signalBus;
		private EffectsManager _effectsManager;

		[Inject]
		public void Inject(SignalBus signalBus, EffectsManager effectsManager) {
			_signalBus = signalBus;
			_effectsManager = effectsManager;
		}

		public void OnRequestZoneCompleted(CompletedRequestReport report)
		{
			_effectsManager.PlayEffect(EffectType.CompleteEffect, report.Position);
			_signalBus.Fire(new RequestCompletedSignal(report));
		}
	}
}
