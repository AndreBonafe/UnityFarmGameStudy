using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFront : MonoBehaviour
{
    private Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            player.IsNextToWaterFront = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            player.IsNextToWaterFront = false;
        }
    }
}
