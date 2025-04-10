using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly_CSharp.Assets.Scripts.InventorySystem;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _size = 8;
        [SerializeField] private List<InventorySlot> _slots;
        private int _activeSlotIndex;

        public int Size => _size;
        public List<InventorySlot> Slots => _slots;

        public int ActiveSlotIndex
        {
            get => _activeSlotIndex;
            private set
            {
                _slots[_activeSlotIndex].IsActive = false;
                _activeSlotIndex = value < 0 ? _size - 1 : value % Size;
                _slots[_activeSlotIndex].IsActive = true; 
            } 
        }

        private void OnValidate() {
            AdjustSize();
        }

        private void Awake() {
            if (_size > 0)
            {
                _slots[0].IsActive = true;
            }
        }

        private void AdjustSize()
        {
            if (_slots == null) _slots = new List<InventorySlot>();
            if (_slots.Count > _size) _slots.RemoveRange(_size, _slots.Count - _size);
            if (_slots.Count < _size) _slots.AddRange(new InventorySlot[_size - _slots.Count]);
        }

        public bool IsFull()
        {
            return _slots.Count(slot => slot.HasItem) >= _size;
        }

        public bool CanAcceptItem(ItemStack itemStack)
        {
            var slotWithStackableItem = FindSlot(itemStack.Item, true);
            return !IsFull() || slotWithStackableItem != null;
        }

        private InventorySlot FindSlot(ItemDefinition item, bool onlyStackabe = false)
        {
            return _slots.FirstOrDefault(slot => slot.Item == item && (item.IsStackble || !onlyStackabe));
        }

        public bool HasItem(ItemStack itemStack, bool checkNumberOfItems = false)
        {
            var itemSlot = FindSlot(itemStack.Item);
            if (itemSlot == null) return false;
            if (!checkNumberOfItems) return true;
            if (itemSlot.Item.IsStackble)
            {
                return itemSlot.NumberOfItems >= itemStack.NumberOfitems;
            }
            return _slots.Count(slot => slot.Item == itemStack.Item) >= itemStack.NumberOfitems;
        }

        public ItemStack AddItem(ItemStack itemStack)
        {
            var relevantSlot = FindSlot(itemStack.Item, true);
            if (IsFull() && relevantSlot == null) {
                throw new InventoryException(InventoryOperation.Add, "Inventory is Full");

            }

            if (relevantSlot != null)
            {
                relevantSlot.NumberOfItems += itemStack.NumberOfitems;
            } else {
                relevantSlot = _slots.First(slot => !slot.HasItem);
                relevantSlot.State = itemStack;
            }
            return relevantSlot.State;
        }

        public ItemStack RemoveItem(int atIndex, bool spawn = false)
        {
            if (!_slots[atIndex].HasItem)
            {
                throw new InventoryException(InventoryOperation.Remove, "Slot is Empty");
            }

            if (spawn && TryGetComponent<GameItemSpawner>(out var itemSpawner))
            {
                itemSpawner.SpawnItem(_slots[atIndex].State);
            }

            ClearSlot(atIndex);
            return new ItemStack();
        }

        public ItemStack RemoveItem(ItemStack itemStack)
        {
            var itemSlot = FindSlot(itemStack.Item) ?? throw new InventoryException(InventoryOperation.Remove, "No Item in inventory");
            if (itemSlot.Item.IsStackble && itemSlot.NumberOfItems < itemStack.NumberOfitems)
            {
                throw new InventoryException(InventoryOperation.Remove, "Not Enough Items");
            }
            itemSlot.NumberOfItems -= itemStack.NumberOfitems;
            if (itemSlot.Item.IsStackble && itemSlot.NumberOfItems > 0)
            {
                return itemSlot.State;
            }

            itemSlot.Clear();
            return new ItemStack();
        }

        public void ClearSlot(int atIndex)
        {
            _slots[atIndex].Clear();
        }

        public void ActivateSlot(int atIndex)
        {
            ActiveSlotIndex = atIndex;
        }

        public InventorySlot GetActiveSlot()
        {
            return _slots[ActiveSlotIndex];
        }
    }
}
