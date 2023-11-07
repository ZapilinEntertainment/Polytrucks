using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class TEST_AddTrailer : MonoBehaviour
	{
		[SerializeField] private Truck _truck;
        private Trailer.Factory _trailerFactory;

        [Inject]
        public void Inject(Trailer.Factory factory) => _trailerFactory = factory;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var trailer = _trailerFactory.Create();
                _truck.AddTrailer(trailer);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _truck.RemoveTrailer();
            }
        }
    }
}
