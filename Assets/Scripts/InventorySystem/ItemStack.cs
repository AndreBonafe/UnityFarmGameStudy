
using System;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    [Serializable]
    public class ItemStack
    {
        [SerializeField] private ItemDefinition _item;
        [SerializeField] private int _numberOfItems;

        public bool IsStackble => _item != null && _item.IsStackble;
        public ItemDefinition Item => _item;

        public bool IsTool => _item != null && _item.IsTool;

        public int NumberOfitems
        {
            get => _numberOfItems;
            set
            {
                value = value < 0 ? 0 : value;
                _numberOfItems = IsStackble ? value : 1;
            }
        }

        public ItemStack(ItemDefinition item, int numberOfItems)
        {
            _item = item;
            NumberOfitems = numberOfItems;
        }

        public ItemStack() {}
    }

}
