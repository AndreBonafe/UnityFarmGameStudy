using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assembly_CSharp.Assets.Scripts.InventorySystem.UI
{
    public class UI_InventorySlot : MonoBehaviour
    {
    [SerializeField]
            private Inventory _inventory;
            
            [SerializeField]
            private int _inventorySlotIndex;
            
            [SerializeField]
            private Image _itemIcon;
            
            [SerializeField]
            private Image _activeIndicator;

            [SerializeField]
            private TMP_Text _numberOfItems;

            [SerializeField]
            private Slider _slider;

            [SerializeField]
            private Image _frame;

            [SerializeField]
            private PlayerItens _playerItens;

            private InventorySlot _slot;

            private float _currentVelocity = 1f;

            private void Start()
            {
                AssignSlot(_inventorySlotIndex);
            }

            public void AssignSlot(int slotIndex)
            {
                if (_slot != null) _slot.StateChanged -= OnStateChanged;
                _inventorySlotIndex = slotIndex;
                if (_inventory == null) _inventory = GetComponentInParent<UI_Inventory>().Inventory;
                if (_playerItens == null) _playerItens = GetComponentInParent<UI_Inventory>().PlayerItens;
                _slot = _inventory.Slots[_inventorySlotIndex];
                _slot.StateChanged += OnStateChanged;
                UpdateViewState(_slot.State, _slot.IsActive);
            }

            private void UpdateViewState(ItemStack state, bool active)
            {
                _activeIndicator.enabled = active;
                var item = state?.Item;
                var hasItem = item != null;
                var isStackble = hasItem && item.IsStackble;
                _itemIcon.enabled = hasItem;
                _frame.enabled = hasItem && state.Item.Name == "Watering Can";
                _numberOfItems.enabled = isStackble;
                if (!hasItem) return;

                _itemIcon.sprite = item.UiSprite;
                if (isStackble) _numberOfItems.SetText(state.NumberOfitems.ToString());
                if (state.Item.Name == "Watering Can")
                {
                    UpdateWaterLevel();
                }
            }

            private void OnStateChanged(object sender, InventorySlotChangedArgs args)
            {
                UpdateViewState(args.NewState, args.Active);
            }

            public void UpdateWaterLevel()
            {
                _slider.maxValue = _playerItens.MaxWater;
                _slider.value = _playerItens.CurrentWater;
            }
    }
}
