using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [Serializable]
    public struct RaycastWheel
    {
        public Transform WheelModel;
        public Transform SuspensionPoint;
        public bool IsMotor, IsSteer, HasBrakes;
        public float WheelRadius, ScaleCf;

        public void SetSuspensionCurrentLength(float length) {
            WheelModel.position = SuspensionPoint.position + Vector3.down * ( length - WheelRadius);
        }
        public void SetSteer(float x) => WheelModel.localRotation = Quaternion.Euler(WheelModel.localRotation.eulerAngles.x, x, 0f);
        public void Spin(float x) => WheelModel.Rotate(Vector3.right * x, Space.Self);
    }
}
