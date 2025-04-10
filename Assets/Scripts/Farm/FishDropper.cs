using System.Collections;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Scripts.InventorySystem;
using Assets.Scripts.InventorySystem;
using UnityEngine;

public class FishDropper : MonoBehaviour
{
    [SerializeField] private GameItem _fishPrefab;
    public void InstantiateFish()
    {
        if (TryGetComponent<GameItemSpawner>(out var itemSpawner))
        {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            double directionX = player.transform.position.x - transform.position.x;
            itemSpawner.SpawnItem(_fishPrefab.Stack, (float)directionX);
        }
    }
}
