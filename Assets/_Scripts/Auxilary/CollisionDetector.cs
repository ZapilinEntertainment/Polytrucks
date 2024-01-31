using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class CollisionDetector : MonoBehaviour
	{
		private int _eventCollidersMask;
		private Action<CollisionResult> _hitAction;
		public void Setup(int collisionEventsMask, Action<CollisionResult> hitAction)
		{
			_hitAction= hitAction;
			_eventCollidersMask = collisionEventsMask;
		}

        private void OnCollisionEnter(Collision collision)
        {
			if (( _eventCollidersMask & (1 << collision.collider.gameObject.layer)) != 0)
			{
				_hitAction?.Invoke(new CollisionResult(collision.impulse.magnitude * GameConstants.WALL_HIT_DAMAGE_CF, collision.collider.GetInstanceID()));
			}
        }
    }

	public class CollisionResult
	{
		public readonly float Impulse;
		public readonly int ColliderID;
		public CollisionResult(float impulse, int colliderID)
		{
			Impulse = impulse;
			ColliderID = colliderID;
		}
	}
}
