using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class TrailerConnector : ITrailerConnectionPoint
	{
		private readonly CachedVehiclesService _cachingService;
        private readonly Truck _truck;
        private List<Trailer> _trailers; 
        public bool HaveTrailers { get; private set; } = false;
        public bool IsTeleporting { get; private set; } = false;
        public bool GarageView = false;
        public Rigidbody Rigidbody => _truck.Rigidbody;
        public IReadOnlyList<Trailer> TrailersList => _trailers;

        public TrailerConnector(Truck truck, CachedVehiclesService cachedVehiclesService)
		{
            _truck = truck;
			_cachingService = cachedVehiclesService;
            _truck.OnVehicleDisposeEvent += RemoveAllTrailers;
            _truck.OnVisibilityChangedEvent += SetTrailersVisibility;
		}

        public void SetTrailersVisibility(bool x)
        {
            if (GarageView & x) x = false;
            if (HaveTrailers)
            {
                foreach (var trailer in _trailers) { trailer.SetVisibility(x); }
            }
        }       

        public void AddTrailer(Trailer trailer)
        {
            if (_trailers == null) _trailers = new List<Trailer>();
            HaveTrailers = true;
            _trailers.Add(trailer);
            // _collidersHandler.AddCollider(trailer.Collider);

            _truck.OnTrailerConnected(trailer);

            int count = _trailers.Count;
            if (count == 1) trailer.OnTrailerConnected(this);
            else trailer.OnTrailerConnected(_trailers[count - 2]);
        }
        public void RemoveTrailer()
        {
            if (!HaveTrailers) return;
            int count = _trailers?.Count ?? 0;
            count--;
            var trailer = _trailers[count];

            _truck.OnTrailerDisconnected(trailer);
            if (trailer.TryGetStorage(out var storage)) storage.MakeEmpty();
            _cachingService.CacheTrailer(trailer);
            _trailers.RemoveAt(count);
            HaveTrailers = count != 0;
        }
        private void RemoveAllTrailers()
        {
            if (!HaveTrailers) return;
            else
            {
                if (_trailers.Count == 1) RemoveTrailer();
                else
                {
                    foreach (var trailer in _trailers)
                    {
                        RemoveTrailer();
                    }
                }
            }
            
            _trailers.Clear();
            HaveTrailers = false;
        }

        public VirtualPoint CalculateTrailerPosition(float distance)
        {
            var truckPoint = _truck.FormVirtualPoint();
            var rotation = truckPoint.Rotation;
            return new VirtualPoint(truckPoint.Position + rotation * (distance * Vector3.back), rotation);
        }
        public void GetTrailersBounds(ref List<Vector3> list)
        {
            foreach (var trailer in _trailers)
            {
                var collider = trailer.Collider;
                if (collider != null)
                {
                    list.Add(collider.bounds.min);
                    list.Add(collider.bounds.max);
                }
            }
        }

        public class Factory : PlaceholderFactory<Truck,TrailerConnector> {

        }
		public class Handler
		{
            private readonly Truck _truck;
			private TrailerConnector _trailerConnector;
			private readonly TrailerConnector.Factory _factory;
            public bool IsActivated { get; private set; } = false;

			public Handler(Truck truck, TrailerConnector.Factory factory)
			{
				_factory = factory;
                _truck = truck;
			}

			public TrailerConnector Connector
			{
				get
				{
                    if (!IsActivated)
                    {
                        _trailerConnector = _factory.Create(_truck);
                        IsActivated = _trailerConnector != null;
                    }
					return _trailerConnector;
				}
			}

            public class Factory : PlaceholderFactory<Truck, Handler> { }
		}
	}
}
