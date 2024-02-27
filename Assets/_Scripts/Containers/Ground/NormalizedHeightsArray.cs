using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class NormalizedHeightsArray
    {
        private readonly float[] _values;
        public readonly int Length;

        public NormalizedHeightsArray(int size)
        {
            _values = new float[size];
            Length = size;
        }
        public float this[int index]
        {
            get => _values[index];
            set => _values[index] = Mathf.Clamp(value, -1f, 1f);
        }

        public byte[] ToBytesArray()
        {
            var bytes = new byte[Length];
            for (int i = 0; i < Length; i++)
            {
                bytes[i] = (byte)(_values[i] * 127f + 127f);
            }
            return bytes;
        }
    }
}
