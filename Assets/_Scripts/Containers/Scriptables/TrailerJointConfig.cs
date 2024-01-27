using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/TrailerJointConfig")]
    public class TrailerJointConfig : ScriptableObject
	{
		[System.Serializable]
		private struct SoftJointLimitSerialized
		{
			public float Limit, Bounciness, ContactDistance;
			public SoftJointLimitSerialized(SoftJointLimit limit)
			{
				Limit = limit.limit;
				Bounciness = limit.bounciness;
				ContactDistance = limit.contactDistance;
			}
			public SoftJointLimit FormLimit() => new SoftJointLimit() { limit = Limit, bounciness = Bounciness, contactDistance = ContactDistance };
		}

		[SerializeField] private Vector3 Anchor;
        [SerializeField] private Vector3 Axis;
        [SerializeField] private Vector3 ConnectedAnchor;
        [SerializeField] private Vector3 SecondaryAxis;
        [SerializeField] private SoftJointLimitSerialized LinearLimit;
        [SerializeField] private SoftJointLimitSerialized LowAngularXLimit;
        [SerializeField] private SoftJointLimitSerialized HighAngularXLimit;
		[SerializeField] private SoftJointLimitSerialized AngularYLimit;
        [SerializeField] private float MassScale;
        [SerializeField] private float ConnectedMassScale;

		public void FillValuesTo(ConfigurableJoint joint)
		{
			joint.anchor = Anchor;
			joint.axis = Axis;
			joint.connectedAnchor = ConnectedAnchor;
			joint.secondaryAxis = SecondaryAxis;
			joint.linearLimit = LinearLimit.FormLimit();
			joint.lowAngularXLimit= LowAngularXLimit.FormLimit();
			joint.highAngularXLimit= HighAngularXLimit.FormLimit();
			joint.angularYLimit= AngularYLimit.FormLimit();

			joint.massScale = MassScale;
			joint.connectedMassScale = ConnectedMassScale;
			joint.enableCollision= true;
		}
		public void SaveValuesFrom(ConfigurableJoint joint)
		{
			Anchor = joint.anchor;
			Axis = joint.axis;
			ConnectedAnchor = joint.connectedAnchor;
			SecondaryAxis = joint.secondaryAxis;
			LinearLimit = new SoftJointLimitSerialized(joint.linearLimit);
			LowAngularXLimit= new SoftJointLimitSerialized(joint.lowAngularXLimit);
			HighAngularXLimit= new SoftJointLimitSerialized(joint.highAngularXLimit);
			AngularYLimit= new SoftJointLimitSerialized(joint.angularYLimit);

			MassScale = joint.massScale;
			ConnectedMassScale = joint.connectedMassScale;
		}

		public void Test()
		{
			//Debug.Log(LinearLimit.limit);
		}
    }
}
