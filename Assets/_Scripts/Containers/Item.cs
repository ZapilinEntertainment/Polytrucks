using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public enum ItemType : byte { Unidentified, Crate}
    public static class ItemTypeExtension
    {
        public static int ToMask(this ItemType itemType) => 1 << (int)itemType;
        public static int FormMask(this ItemType[] itemTypes)
        {
            int x = 0;
            foreach (var item in itemTypes)
            {
                int mask = item.ToMask();
                if ((x & mask) == 0) x += mask;
            }
            return x;
        }
    }

    [System.Serializable]
    public struct Item 
    {
        public ItemType ItemType;
        public int Value;
        public static Item EmptyItem => new Item(ItemType.Unidentified, -1);

        public Item(ItemType i_type, int val)
        {
            ItemType = i_type;
            Value = val;
        }
        public string GetItemName()
        {
            switch (ItemType)
            {
                case ItemType.Crate: return "Crate";
                default: return string.Empty;
            }
        }
        public int GetCost() => 10;
    }
}
