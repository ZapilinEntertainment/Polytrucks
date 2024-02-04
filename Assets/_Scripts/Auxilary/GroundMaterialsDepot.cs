using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Pool;

namespace ZE.Polytrucks {

	public readonly struct DeformedMaterialID
	{
		public readonly GroundType GroundType;

		public DeformedMaterialID(GroundType type)
		{
			this.GroundType = type;
		}

		public static bool operator ==(DeformedMaterialID id1, DeformedMaterialID id2) => id1.GroundType == id2.GroundType;
        public static bool operator !=(DeformedMaterialID id1, DeformedMaterialID id2) => !(id1 == id2);
        public override bool Equals(object ob)
        {
            if (ob is DeformedMaterialID)
            {
                DeformedMaterialID c = (DeformedMaterialID)ob;
				return c == this;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return GroundType.GetHashCode();
        }
    }
    public readonly struct GroundMaterialContainer
    {
        public readonly DeformedMaterialID ID;
        public readonly Material Material;

        public GroundMaterialContainer(DeformedMaterialID id, Material material)
        {
            ID = id;
            Material = material;
        }
    }
    public sealed class GroundMaterialsDepot 
	{
		private List<GroundMaterialContainer> _cachedMaterials = new List<GroundMaterialContainer>();

		public bool TryReuseMaterial(DeformedMaterialID id, out Material material)
		{
			int count = _cachedMaterials.Count;
			if (count != 0)
			{
				for (int i = 0; i < count; i++)
				{
					var item = _cachedMaterials[i];
					if (item.ID == id)
					{
						material = item.Material;
						_cachedMaterials.RemoveAt(i);
						return true;
					}
				}
			}
            material = null;
            return false;
        }
		public void CacheMaterial(GroundMaterialContainer container)
		{
			_cachedMaterials.Add(container);
		}
	}
}
