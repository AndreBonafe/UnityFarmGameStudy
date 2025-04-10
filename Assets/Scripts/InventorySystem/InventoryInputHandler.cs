using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class InventoryInputHandler : MonoBehaviour
    {
        private Inventory _inventory;

        private KeyCode[] keyCodes = {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9
        };

        private void Awake() {
            _inventory = GetComponent<Inventory>();
        }

        private void Update() {
            OnNextItem();
            OnPreviousItem();
            OnThrowItem();
            OnChangeItem();
        }

        private void OnThrowItem()
        {
            if (_inventory.GetActiveSlot().HasItem)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _inventory.RemoveItem(_inventory.ActiveSlotIndex, true);
                }
            }
        }

        private void OnChangeItem()
        {
            for (int i = 0; i < keyCodes.Length; i += 1)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    _inventory.ActivateSlot(i);
                }
            }
        }

        private void OnNextItem()
        {
            if (Input.mouseScrollDelta.y < 0) {
                _inventory.ActivateSlot(_inventory.ActiveSlotIndex + 1);
            }
        }

        private void OnPreviousItem()
        {
            if (Input.mouseScrollDelta.y > 0) {
                _inventory.ActivateSlot(_inventory.ActiveSlotIndex - 1);
            }
        }
    }
}
