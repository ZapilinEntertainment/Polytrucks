using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/StorageConfiguration")]
    public sealed class StorageConfiguration : ScriptableObject, IStorageConfiguration
	{
		[field: SerializeField] public Vector3Int Size { get; private set; } = new Vector3Int(4,4,4);
		[field: SerializeField] public float ModelScale { get; private set; } = 1f;
		[field: SerializeField] public float Gap { get; private set; } = 0.1f;
        public int Capacity => Size.x * Size.y * Size.z;
    }

    public interface IStorageConfiguration
    {
        public Vector3Int Size { get; }
        public float ModelScale { get; }
         public float Gap { get; }
        public int Width => Size.x;
        public int Height => Size.y;
        public int Length => Size.z;
        public int ItemsInLayer => Width * Length;
        public int Capacity => Size.x * Size.y * Size.z;
    }

    public struct VirtualStorageConfiguration : IStorageConfiguration
    {
        private readonly Vector3Int _size;
        private readonly float _modelScale;
        private readonly float _gap;
        public Vector3Int Size => _size;
        public float ModelScale => _modelScale;
        public float Gap => _gap;
        
        public VirtualStorageConfiguration(Vector3Int size, float modelScale, float gap)
        {
            _size= size;
            _modelScale= modelScale;
            _gap= gap;
        }
    }
}
