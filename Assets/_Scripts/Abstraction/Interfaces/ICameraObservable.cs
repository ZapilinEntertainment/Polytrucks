using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public interface ICameraObservable
    {
        public float HeightViewCf { get; }
        public float HeightSpeedOffsetCf { get; }
    }

    public class CameraObservableSettings : ICameraObservable {
        public float HeightViewCf => 1f;
        public float HeightSpeedOffsetCf => 1f;
    }
}
