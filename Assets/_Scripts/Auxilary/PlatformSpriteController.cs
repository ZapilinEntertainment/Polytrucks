using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public sealed class PlatformSpriteController : PlatformSwitchableRenderer
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Color _readyColor = Color.green, _blockedColor = Color.yellow, _movingColor = Color.blue, _disabledColor = Color.gray;
        public override void SetState(PlatformState state)
        {
            switch (state)
            {
                case PlatformState.Ready: _renderer.color = _readyColor;break;
                case PlatformState.Blocked: _renderer.color = _blockedColor; break;
                case PlatformState.Moving: _renderer.color = _movingColor; break;
                case PlatformState.Disabled: _renderer.color = _disabledColor; break;
                default: _renderer.color = Color.white; break;
            }
        }
    }
}
