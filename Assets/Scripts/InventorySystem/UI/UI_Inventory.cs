using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.InventorySystem;
using UnityEditor;
using UnityEngine;

namespace Assembly_CSharp.Assets.Scripts.InventorySystem.UI
{
    public class UI_Inventory : MonoBehaviour
    {
        [SerializeField]
        private GameObject _inventorySlotPrefab;

        [SerializeField]
        private Inventory _inventory;

        [SerializeField]
        private List<UI_InventorySlot> _slots;

        [SerializeField]
        private PlayerItens _playerItens;


        public Inventory Inventory => _inventory;

        public PlayerItens PlayerItens => _playerItens;

        [ContextMenu("Initialize Inventory")]
        private void InitializeInventoryUI()
        {
            if (_inventory == null || _inventorySlotPrefab == null) return;
            _slots = new List<UI_InventorySlot>(_inventory.Size);

            for (var i = 0; i < _inventory.Size; i ++)
            {
                var uiSlot = PrefabUtility.InstantiatePrefab(_inventorySlotPrefab) as GameObject;
                uiSlot.transform.SetParent(transform, false);
                var uiSlotScript = uiSlot.GetComponent<UI_InventorySlot>();
                uiSlotScript.AssignSlot(i);
                _slots.Add(uiSlotScript);
            }
        }
    }
}
