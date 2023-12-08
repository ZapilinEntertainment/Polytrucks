using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class SpriteColourer : ColourableRenderer
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;

        public override void SetColour(Color colour)
        {
            _spriteRenderer.color = colour;
        }
    }
}
