using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CollectibleSpot : MonoBehaviour
	{
		private ObjectsManager _objectsManager;
		[Inject]
		public void Setup(ObjectsManager objectsManager)
		{
			_objectsManager= objectsManager;
		}

        private void Start()
        {
			var crate = _objectsManager.CreateCrate();
			crate.transform.position = transform.position;
        }

        private void OnDrawGizmos()
        {
			Gizmos.DrawSphere(transform.position, GameConstants.CRATE_COLLECT_RADIUS);
        }
    }
}
