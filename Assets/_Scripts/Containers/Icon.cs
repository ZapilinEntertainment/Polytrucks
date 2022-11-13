using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public enum Icon : byte { Undefined, MoneyIcon}
    public static class IconExtension
    {
        private static Dictionary<Icon, Sprite> _sprites = new Dictionary<Icon, Sprite>();
        public static Sprite GetSprite(this Icon icon)
        {
            Sprite sprite;
            if (!_sprites.TryGetValue(icon, out sprite) )
            {
                sprite = Resources.Load<Sprite>("Sprites/" + icon.ToString());
                _sprites.Add(icon, sprite);
            }
            return sprite;
        }
    }
}
