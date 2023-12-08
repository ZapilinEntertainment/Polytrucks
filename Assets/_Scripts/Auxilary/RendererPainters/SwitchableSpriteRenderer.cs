using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public sealed class SwitchableSpriteRenderer : SwitchableRenderer
    {
        [SerializeField] private SpriteRenderer _spriteRender;
        [SerializeField] private Color _activeColor = Color.white, _disabledColor = Color.gray;
        public override void SetActivity(bool x)
        {
            _spriteRender.color = x ? _activeColor : _disabledColor;
        }
    }
}
