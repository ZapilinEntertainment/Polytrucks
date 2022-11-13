using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public interface IProgressionObject 
    {
        public bool IsProgressing { get; }
        public float Progress { get; }
        public Vector3 IndicatorPosition { get; }
        
    }
}
