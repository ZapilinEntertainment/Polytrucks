using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using DG.Tweening;

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
        public static void Teleport(Rigidbody rigidbody, VirtualPoint point, Action OnTeleportationComplete = null) {
            var waiter = new RigidbodyWaiter(rigidbody, OnTeleportationComplete);
            rigidbody.MoveRotation(point.Rotation);
            var tw = rigidbody.DOMove(point.Position, 0f);
			if (OnTeleportationComplete != null) tw.OnComplete(() => waiter.Complete());		
                     		
        }
	}
}
