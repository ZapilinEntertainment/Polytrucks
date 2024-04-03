using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class ColouredMaterialsDepot
	{
		private readonly Dictionary<int, Material> _colouredMaterials= new (), _originalMaterials = new ();

		public int CreateColouredMaterial(in Material originalMaterial, Color color, out Material colouredMaterial)
		{
			int originalMaterialHash = originalMaterial.GetHashCode();
			if (!_originalMaterials.ContainsKey(originalMaterialHash)) _originalMaterials.Add(originalMaterialHash, originalMaterial);

			colouredMaterial = GetOrCreateColouredMaterial(originalMaterial, color, originalMaterialHash);

			return originalMaterialHash;
		}
		public Material CreateColouredMaterial(int originalMaterialID, Color color)
		{
			if (!_originalMaterials.TryGetValue(originalMaterialID, out var originalMaterial))
			{
				Debug.LogError("Coloured materials depot cannot find the original material");
				return default;
			}
			else
			{			
				return GetOrCreateColouredMaterial(originalMaterial, color);
			}            
        }
		private Material GetOrCreateColouredMaterial(in Material originalMaterial, Color color, int originalMaterialCachedHash = 0)
		{
            var hash = new HashCode();
            hash.Add(originalMaterialCachedHash == 0 ? originalMaterial.GetHashCode() : originalMaterialCachedHash);
            hash.Add(color.GetHashCode());
            int colouredMaterialKey = hash.ToHashCode();

            Material colouredMaterial;
            if (!_colouredMaterials.TryGetValue(colouredMaterialKey, out colouredMaterial))
            {
                colouredMaterial = new Material(originalMaterial);
                colouredMaterial.color = color;
                _colouredMaterials.Add(colouredMaterialKey, colouredMaterial);
            }
			return colouredMaterial;
        }

		public Material GetOriginalMaterial(int hash)
		{
			if (_originalMaterials.TryGetValue(hash, out var material)) return material;
			else return default;
		}
	}
}
