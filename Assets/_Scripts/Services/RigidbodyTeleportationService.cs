using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;


namespace ZE.Polytrucks {
	public class RigidbodyTeleportationService 	{
		private class RigidbodyWaiter
        {
            public readonly Rigidbody Rigidbody;
            public readonly Action CompleteCallback;

            public RigidbodyWaiter(Rigidbody rigidbody, Action completeCallback)
            {
                Rigidbody = rigidbody;
                CompleteCallback = completeCallback;
            }
            public void Complete()
            {
                Rigidbody.ResetInertiaTensor();
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
                CompleteCallback?.Invoke();
            }
        }

        private static TeleportationHandler _teleportationHandler;
        public  static TeleportationHandler TeleportationHandler { get { if (_teleportationHandler == null) _teleportationHandler = new GameObject("teleportationHandler").AddComponent<TeleportationHandler>();  return _teleportationHandler; } }
        public static void Teleport(Rigidbody rigidbody, VirtualPoint point, Action OnTeleportationComplete = null) {
            var waiter = new RigidbodyWaiter(rigidbody, OnTeleportationComplete);
            rigidbody.MoveRotation(point.Rotation);
            rigidbody.MovePosition(point.Position);
            TeleportationHandler.StartCoroutine(TeleportCoroutine(waiter));
        }

        private static IEnumerator TeleportCoroutine(RigidbodyWaiter waiter)
        {
            yield return new WaitForFixedUpdate();
            waiter.Complete();
        }
	}
}
