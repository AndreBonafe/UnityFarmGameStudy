using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.InventorySystem;
using UnityEditor;
using UnityEngine;

namespace Assembly_CSharp.Assets.Scripts.InventorySystem
{
    public class GameItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _itemBasePrefab;

        public void SpawnItem(ItemStack itemStack, float direction = 0)
        {
            if (_itemBasePrefab == null) return;
            var item = PrefabUtility.InstantiatePrefab(_itemBasePrefab) as GameObject;
            item.transform.position = transform.position;
            var gameItemScript = item.GetComponent<GameItem>();
            gameItemScript.SetStack(new ItemStack(itemStack.Item, itemStack.NumberOfitems));
            if (direction == 0)
            {
                gameItemScript.Throw(transform.rotation.y);
            } else {
                gameItemScript.Throw(direction);
            }
        }
    }
}
