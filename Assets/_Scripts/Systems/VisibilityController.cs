using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public enum UpdateInterval : byte
	{
		PerFrame, PerFixedFrame, PerSecond
	}
	public interface IVisibilityListener
	{
		public Vector3 Position { get; }
		public void OnBecameVisible();
		public void OnBecameInvisible();
	}
	public struct VisibilityConditions
	{
		public IVisibilityListener Listener;
		public UpdateInterval UpdateInterval;
		public float VisibilityRadius;

		public VisibilityConditions(IVisibilityListener listener, UpdateInterval updateInterval, float radius)
		{
			Listener = listener;
			UpdateInterval = updateInterval;
			VisibilityRadius = radius;
		}
	}


	public sealed class VisibilityController : MonoBehaviour
	{
		private class VisibilityListener
		{
			public readonly IVisibilityListener Listener;
			private readonly float _visibilityRadius;
			private bool _isVisible = false;

			public VisibilityListener (VisibilityConditions conditions)
			{
				Listener = conditions.Listener;
				_visibilityRadius = conditions.VisibilityRadius;
			}

			public void CheckVisibility(in Vector3 playerPosition)
			{
                bool isVisible = (playerPosition - Listener.Position).sqrMagnitude < _visibilityRadius * _visibilityRadius;
				if (isVisible != _isVisible)
				{
					_isVisible = isVisible;
					if (_isVisible) Listener.OnBecameVisible();
					else Listener.OnBecameInvisible();
				}
			}
		}
		private sealed class ListenersHost
		{
			public bool HaveListeners { get; private set; } = false;
			private List<VisibilityListener> _listeners = new List<VisibilityListener>();

			public void Update(Vector3 playerPosition)
			{
                foreach (var listener in _listeners)
                {
                    listener.CheckVisibility(playerPosition);
                }
            }
			public void AddListener(VisibilityListener listener)
			{
				_listeners.Add(listener);
				HaveListeners = true;
			}
			public bool TryRemoveListener(IVisibilityListener listener)
			{
				if (!HaveListeners) return false;
				int x = -1;
				foreach (var item in _listeners)
				{
                    x++;
                    if (item.Listener == listener)
					{
						break;
					}
				}
				if (x > 0)
				{
					_listeners.RemoveAt(x);
					HaveListeners = _listeners.Count > 0;
					return true;
				}
				else return false;
			}
        }

		private bool _haveFrameListeners = false, _haveFixedListeners = false;
		private Vector3 _playerPosition;
		private PlayerController _player;
		private ListenersHost _frameHost = new ListenersHost(), _fixedFrameHost = new ListenersHost();

		[Inject]
		public void Inject(PlayerController player)
		{
			_player = player;
		}

		private ListenersHost GetHost(UpdateInterval interval) => interval == UpdateInterval.PerFixedFrame ? _fixedFrameHost : _frameHost;
        public void AddListener(VisibilityConditions conditions)
		{
			var listener = new VisibilityListener(conditions);
			GetHost(conditions.UpdateInterval).AddListener(listener);

			OnListenersCountChanged();
		}
		public void RemoveListener(IVisibilityListener listener, UpdateInterval interval)
		{
			if (GetHost(interval).TryRemoveListener(listener))    OnListenersCountChanged();
        }
		private void OnListenersCountChanged()
		{
			_haveFrameListeners = _frameHost.HaveListeners;
			_haveFixedListeners = _fixedFrameHost.HaveListeners;
		}

        private void Update()
        {
			_playerPosition = _player.Position;
			if (_haveFrameListeners) _frameHost.Update(_playerPosition);
        }
        private void FixedUpdate()
        {
            if (_haveFixedListeners) _fixedFrameHost.Update(_playerPosition);
        }

    }
}
