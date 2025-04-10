using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItens : MonoBehaviour
{
    [SerializeField] private int totalWood;
    [SerializeField] private float currentWater;
    [SerializeField] private float maxWater;
    private readonly float initialWater = 50;

    private void Start() {
        maxWater = initialWater;
    }

    public int TotalWood { get => totalWood; set => totalWood = value; }
    public float CurrentWater { get => currentWater; set => currentWater = value; }
    public float MaxWater { get => maxWater; set => maxWater = value; }
}
