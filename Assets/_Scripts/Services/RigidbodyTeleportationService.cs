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
                Rigidbody.isKinematic = false;
                CompleteCallback?.Invoke();
            }
        }

        private static TeleportationHandler _teleportationHandler;
        public  static TeleportationHandler TeleportationHandler { get { if (_teleportationHandler == null) _teleportationHandler = new GameObject("teleportationHandler").AddComponent<TeleportationHandler>();  return _teleportationHandler; } }
        public static void Teleport(Rigidbody rigidbody, VirtualPoint point, Action OnTeleportationComplete = null) {
            var waiter = new RigidbodyWaiter(rigidbody, OnTeleportationComplete);
            rigidbody.isKinematic = true;
            rigidbody.MoveRotation(point.Rotation);
            rigidbody.MovePosition(point.Position);
            TeleportationHandler.StartCoroutine(TeleportCoroutine(waiter));
        }
        public static void Teleport(IReadOnlyList<Rigidbody> list, VirtualPoint startPoint,VirtualPoint targetPoint, Action OnTeleportationComplete = null)
        {
            int count = list.Count; 
            var participants = new TeleportationParticipant[count];
            var transformMatrix = startPoint.ToTransformMatrix().inverse;

            for (int i = 0; i < count;i++)
            {
                var rigidbody = list[i];
                participants[i] = new TeleportationParticipant(rigidbody, transformMatrix.MultiplyPoint3x4(rigidbody.position));
            }            

            TeleportationHandler.StartCoroutine(TeleportCoroutine(participants, targetPoint, OnTeleportationComplete));
        }

        private static IEnumerator TeleportCoroutine(RigidbodyWaiter waiter)
        {
            yield return new WaitForFixedUpdate();
            waiter.Complete();
        }
        private static IEnumerator TeleportCoroutine(TeleportationParticipant[] list, VirtualPoint newPoint, Action onTeleportReady)
        {
            yield return new WaitForFixedUpdate();

            var matrix = newPoint.ToTransformMatrix();

            foreach (var participant in list)
            {
                var rigidbody = participant.Rigidbody;
                rigidbody.MovePosition(matrix.MultiplyPoint3x4( participant.LocalPosition));                    
                rigidbody.MoveRotation(newPoint.Rotation);                
            }
            yield return new WaitForFixedUpdate();
            foreach (var participant in list)
            {
                participant.StopTeleportation();
            }
            onTeleportReady?.Invoke();
        }

        private class TeleportationParticipant
        {
            public readonly Rigidbody Rigidbody;
            public readonly Vector3 LocalPosition;

            public TeleportationParticipant(Rigidbody rigidbody, Vector3 localPosition)
            {
                this.Rigidbody = rigidbody;
                this.LocalPosition = localPosition;

                rigidbody.isKinematic = true;                
            }

            public void StopTeleportation()
            {
                Rigidbody.isKinematic = false;
                Rigidbody.ResetInertiaTensor();
                Rigidbody.velocity = Vector3.zero;
                Rigidbody.angularVelocity = Vector3.zero;
            }
        }
	}
}
