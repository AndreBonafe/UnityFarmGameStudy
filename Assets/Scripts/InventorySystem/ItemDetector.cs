using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
    public class ItemDetector : MonoBehaviour
    {
        private Inventory _inventory;

        private void Awake() {
            _inventory = GetComponentInParent<Inventory>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (
                !col.TryGetComponent<GameItem>(out var gameItem)
                || !_inventory.CanAcceptItem(gameItem.Stack)
            ) return;

            _inventory.AddItem(gameItem.Pick());
        }   
    }

}
