using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class MeshColourer : ColourableRenderer
	{
        [SerializeField] private bool _preserveAlpha = true;
        [SerializeField] private MeshRenderer _renderer;
        
        private bool _originalMaterialSaved = false;
        private int _originalMaterialKey = -1;
        private ColouredMaterialsDepot _colouredMaterialsDepot;

        [Inject]
        public void Inject(ColouredMaterialsDepot depot)
        {
            _colouredMaterialsDepot= depot;
        }

        public override void SetColour(Color colour)
        {
            if (!_originalMaterialSaved)
            {
                if (_preserveAlpha)
                {
                    colour = new Color(colour.r, colour.g, colour.b, _renderer.sharedMaterial.color.a);
                }

                _originalMaterialKey = _colouredMaterialsDepot.CreateColouredMaterial(_renderer.sharedMaterial, colour, out var colouredMaterial);
                _renderer.sharedMaterial = colouredMaterial;
                _originalMaterialSaved = true;
            }
            else
            {
                _renderer.sharedMaterial = _colouredMaterialsDepot.CreateColouredMaterial(_originalMaterialKey, colour);
            }
        }
        public void Uncolour()
        {
            if (_originalMaterialSaved)
            {
                _renderer.sharedMaterial = _colouredMaterialsDepot.GetOriginalMaterial(_originalMaterialKey);
            }
        }

    }
}
