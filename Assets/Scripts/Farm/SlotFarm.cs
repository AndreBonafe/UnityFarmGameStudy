using System.Collections;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Scripts.InventorySystem;
using Assembly_CSharp.Assets.Scripts.WorldTime;
using Assets.Scripts.InventorySystem;
using UnityEngine;

public class SlotFarm : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite hole;
    [SerializeField] private Sprite carrotHole;
    [SerializeField] private Sprite plantedSprite;
    [SerializeField] private Sprite wateredSprite;
    [SerializeField] private Sprite _carrotSprite;
    [SerializeField] private WorldTime _worldTime;
    private BoxCollider2D _tileCollider;
    private bool holeOpen;
    private bool isPlanted;
    private bool isWatered;
    private bool _isReady;
    private int _dayPlanted;
    private int _daysPlantedCounter;

    [SerializeField] private Player _player;
    private Inventory _inventory;
    [SerializeField] private ItemStack _carrotSeed;
    [SerializeField] private GameItem _carrotPrefab;

    private void Start() {
        if (_player != null)
        {
            _inventory = _player.GetComponent<Inventory>();
        }
        _tileCollider = GetComponent<BoxCollider2D>();
        _worldTime.DayChanged += IncreasePlantedTimeByDay;
    }

    private void OnDestroy()
    {
        _worldTime.DayChanged -= IncreasePlantedTimeByDay;
    }
    public void onDig()
    {
        holeOpen = true;
        spriteRenderer.sprite = hole;
    }

    private void OnPlant()
    {
        isPlanted = true;
        spriteRenderer.sprite = plantedSprite;
        _dayPlanted = _worldTime.CurrentDay;
        _inventory?.RemoveItem(_carrotSeed);
    }

    private void OnIrrigate()
    {
        isWatered = true;
        spriteRenderer.sprite = wateredSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision && IsFullyInside(_tileCollider, collision))
        {
            if(collision.CompareTag("Shovel") && !holeOpen)
            {
                onDig();
            } 
            if(collision.CompareTag("Hand") && holeOpen && !isPlanted)
            {
                OnPlant();
            }
            if(collision.CompareTag("Water") && holeOpen && isPlanted && !isWatered)
            {
                OnIrrigate();
            }
            if(collision.CompareTag("Aim") && _isReady)
            {
                OnCollect();
            }
        }
    }

    private bool IsFullyInside(Collider2D inner, Collider2D outer)
    {
        Bounds innerBounds = inner.bounds;
        Bounds outerBounds = outer.bounds;

        Vector2[] corners = GetBoundingCorners(outerBounds);
        foreach (Vector2 corner in corners)
        {
            if (!innerBounds.Contains(corner))
            {
                return false;
            }
        }
        return true;
    }

    private Vector2[] GetBoundingCorners(Bounds bounds)
    {
        Vector2 min = bounds.min;
        Vector2 max = bounds.max;

        return new Vector2[]
        {
            new Vector2(min.x, min.y),
            new Vector2(min.x, max.y),
            new Vector2(max.x, min.y),
            new Vector2(max.x, max.y)
        };
    }

    private void OnGrow()
    {
        spriteRenderer.sprite = _carrotSprite;
        isWatered = false;
        _isReady = true;
    }

    private void IncreasePlantedTimeByDay(object sender, int newDay)
    {
        if (isPlanted && !_isReady) {
            if (isWatered && _daysPlantedCounter < 0) {
                _daysPlantedCounter = newDay - _dayPlanted;
                isWatered = false;
                spriteRenderer.sprite = plantedSprite;
            } else if (isWatered && _daysPlantedCounter == 0) {
                OnGrow();
            }
        }
    }

    private void OnCollect()
    {
        isWatered = false;
        isPlanted = false;
        _isReady = false;
        _dayPlanted = 0;
        _daysPlantedCounter = 0;
        spriteRenderer.sprite = null;
        if (TryGetComponent<GameItemSpawner>(out var itemSpawner))
        {
            itemSpawner.SpawnItem(_carrotPrefab.Stack);
        }
    }
}
