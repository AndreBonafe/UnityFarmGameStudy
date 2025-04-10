using System.Collections;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Scripts.WorldTime;
using Assets.Scripts.InventorySystem;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    private Player player;
    private PlayerItens playerItens;
    private Inventory _inventory;
    private float _initialSpeed;
    private Rigidbody2D rig;
    private Vector2 _direction;
    private bool _isRunning;
    private bool _isRolling;
    private bool _isCutting;
    private bool _isDigging;
    private bool _isWatering;
    private bool _isEmptyCan;
    private bool _isPlanting;
    private bool _isInteracting;
    private bool _isInBed;
    private bool _isFishing;
    private bool _startedFishing;
    private bool _caught;
    private bool _isPullingFish;
    private int _caughtCount = 0;
    private Coroutine _fishLoopCoroutine = null;
    // private float _gridSize = 1f;
    // [SerializeField] private BoxCollider2D _selectedTile;
    [SerializeField] private bool _isNextToWater;
    [SerializeField] private bool _isNextToWaterFront;
    [SerializeField] private bool _isFillingWateringCan;
    [SerializeField] private int _catchFishChance;
    [SerializeField] private GameObject _bed;
    [SerializeField] private WorldTime _worldTime;
    [SerializeField] private FishDropper _fishDropper;

    private string selectedItemName;

    public bool isRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    public bool isRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    public float initialSpeed
    {
        get { return _initialSpeed; }
        set { _initialSpeed = value; }
    }

    public Vector2 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public bool IsCutting { get => _isCutting; set => _isCutting = value; }
    public bool IsDigging { get => _isDigging; set => _isDigging = value; }
    public bool IsWatering { get => _isWatering; set => _isWatering = value; }
    public bool IsNextToWater { get => _isNextToWater; set => _isNextToWater = value; }
    public bool IsFillingWateringCan { get => _isFillingWateringCan; set => _isFillingWateringCan = value; }
    public bool IsEmptyCan { get => _isEmptyCan; set => _isEmptyCan = value; }
    public bool IsPlanting { get => _isPlanting; set => _isPlanting = value; }
    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }
    public bool IsInBed { get => _isInBed; set => _isInBed = value; }
    public bool IsFishing { get => _isFishing; set => _isFishing = value; }
    public bool StartedFishing { get => _startedFishing; set => _startedFishing = value; }
    public bool IsNextToWaterFront { get => _isNextToWaterFront; set => _isNextToWaterFront = value; }
    public bool Caught { get => _caught; set => _caught = value; }
    public bool IsPullingFish { get => _isPullingFish; set => _isPullingFish = value; }

    private void Start() {
        _inventory = GetComponentInParent<Inventory>();
        _initialSpeed = speed;
        rig = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        playerItens = GetComponent<PlayerItens>();
    }

    private void Update()
    {
        if (!IsInBed)
        {
            selectTool();
            onInput();
            onRoll();
            onCut();
            onDig();
            onWater();
            onPlant();
            OnInteract();
            onRun();
            OnFishing();
        }
        /* Vector2 nextTileCenter = GetNexGridPosition(transform.position, transform.forward);
        _selectedTile.transform.position = nextTileCenter; */
    }

    private void FixedUpdate() {
        if (!IsInBed && !IsFishing)
        {
            onMove();
        }
    }

    /* private Vector2 GetNexGridPosition(Vector2 playerPosition, Vector2 direction)
    {
        Vector2 nextPosition = playerPosition + direction.normalized * _gridSize;

        float snappedX = Mathf.Round(nextPosition.x / _gridSize) * _gridSize;
        float snappedY = Mathf.Round(nextPosition.y / _gridSize) * _gridSize;

        return new Vector2(snappedX, snappedY);
    } */

    #region Movement

    void onInput()
    {
        if (!IsFishing)
        {
            _direction  = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
    }

    void onMove()
    {
        rig.MovePosition(rig.position + _direction * speed * Time.fixedDeltaTime);
    }

    void onRun()
    {
        if (!IsFishing)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = runSpeed;
                _isRunning = true;
            }
            
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = _initialSpeed;
                _isRunning = false;
            }
        }
    }

    void onRoll()
    {
        if (!IsFishing)
        {
            if(Input.GetMouseButtonDown(1) && !_isRolling)
            {
                _isRolling = true;
                StartCoroutine(ResetRoll());
            }
        }
    }

    IEnumerator ResetRoll()
    {   
        if (isRunning) 
        {
            yield return new WaitForSeconds(0.8f);
        }
        else
        {
            yield return new WaitForSeconds(0.6f);
        }
        _isRolling = false;
    }
    #endregion

    #region Tools
    void onCut()
    {
        if (selectedItemName == "Axe" && player.direction.sqrMagnitude == 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsCutting = true;
            }
            
            if(Input.GetMouseButtonUp(0))
            {
                IsCutting = false;
            }
        }
    }

    void onDig()
    {
        if (selectedItemName == "Shovel" && player.direction.sqrMagnitude == 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsDigging = true;
            }
            
            if(Input.GetMouseButtonUp(0))
            {
                IsDigging = false;
            }
        }
    }

    void selectTool()
    {
        if (_inventory != null)
        {
            var currentInventorySlot = _inventory.Slots[_inventory.ActiveSlotIndex];
            if (currentInventorySlot.Item == null)
            {
                selectedItemName = "";
            }
            if (currentInventorySlot != null && currentInventorySlot.Item && selectedItemName != currentInventorySlot.Item.Name)
            {
                selectedItemName = currentInventorySlot.Item.Name;
            }
        }
    }

    void onWater()
    {
        if (selectedItemName == "Watering Can" && player.direction.sqrMagnitude == 0 && !IsNextToWater && !IsWatering && playerItens.CurrentWater > 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsWatering = true;
                playerItens.CurrentWater -= 5;
                StartCoroutine(ResetWatering());
                _inventory.GetActiveSlot().UpdateUIWater();
            }
        }
        if (selectedItemName == "Watering Can" && player.direction.sqrMagnitude == 0 && IsNextToWater && !IsFillingWateringCan && playerItens.CurrentWater <= playerItens.MaxWater)
        {
            if(Input.GetMouseButtonDown(0))
            {
                playerItens.CurrentWater = playerItens.MaxWater;
                IsFillingWateringCan = true;
                StartCoroutine(ResetFill());
                _inventory.GetActiveSlot().UpdateUIWater();
            }
        }
        if (selectedItemName == "Watering Can" && player.direction.sqrMagnitude == 0 && !IsNextToWater && !IsFillingWateringCan && playerItens.CurrentWater <= 0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsEmptyCan = true;
                StartCoroutine(ResetEmpty());
            }
        }
    }

    private void onPlant()
    {
        if(selectedItemName == "Carrot Seed")
        {
            if(Input.GetMouseButtonDown(0))
            {
                IsPlanting = true;
                StartCoroutine(ResetPlant());
            }
        }
    }

    private void OnInteract()
    {
        if(Input.GetMouseButtonDown(1) && player.direction.sqrMagnitude == 0)
        {
            IsInteracting = true;
            StartCoroutine(ResetInteract());
        }
    }

    private void OnFishing()
    {
        if (selectedItemName == "Fishing Pole" && player.direction.sqrMagnitude == 0 && (IsNextToWater || IsNextToWaterFront))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_isFishing && !Caught && !IsPullingFish)
                {
                    StartFishing();
                }
                else if (_isFishing && Caught)
                {
                    CatchFish();
                }
                else if (_isFishing && !Caught)
                {
                    _isFishing = false;
                    if (_fishLoopCoroutine != null)
                    {
                        StopCoroutine(_fishLoopCoroutine);
                        _fishLoopCoroutine = null;
                    }
                    Debug.Log("Saindo da Pesca");
                }
            }
        }
    }

    private void CatchFish()
    {
        Debug.Log("Pegou o Peixe!");
        _caughtCount = 0;
        IsPullingFish = true;
        StartCoroutine(ResetPullingFish());
        if (_fishLoopCoroutine != null)
        {
            StopCoroutine(_fishLoopCoroutine);
            _fishLoopCoroutine = null;
        }
    }

    private void StartFishing()
    {
        _caughtCount = 0;
        _isFishing = true;
        StartedFishing = true;
        _worldTime.Stop();
        StartCoroutine(ResetStartedFishing());
        if (_fishLoopCoroutine == null)
        {
            _fishLoopCoroutine = StartCoroutine(FishLoop());
        }
    }
    #endregion

    #region Enumerators

    private IEnumerator FishLoop()
    {
        while (_isFishing)
        {
            yield return new WaitForSeconds(2f);
            if (!Caught)
            {
                int value = Random.Range(0, 100);
                Debug.Log(value);
                Caught = value <= _catchFishChance;
            }

            if (Caught)
            {
                Debug.Log("Peixe mordeu, clique!");
                _caughtCount++;

                if (_caughtCount > 3)
                {
                    Debug.Log("Perdeu o peixe!");
                    _caughtCount = 0;
                    Caught = false;
                }
            }
        }

        _fishLoopCoroutine = null;
    }
    private IEnumerator ResetStartedFishing()
    {
        yield return new WaitForSeconds(1f);
        StartedFishing = false;
    }

    private IEnumerator ResetPullingFish()
    {
        yield return new WaitForSeconds(0.3f);
        IsPullingFish = false;
        Caught = false;
        _fishDropper.InstantiateFish();
        StartFishing();
    }



    IEnumerator ResetInteract()
    {
        yield return new WaitForSeconds(0.3f);
        IsInteracting = false;
    }
    
    IEnumerator ResetFill()
    {
        yield return new WaitForSeconds(1);
        IsFillingWateringCan = false;
    }
    IEnumerator ResetWatering()
    {
        yield return new WaitForSeconds(0.6f);
        IsWatering = false;
    }
    IEnumerator ResetEmpty()
    {
        yield return new WaitForSeconds(0.3f);
        IsEmptyCan = false;
    }
    IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(0.3f);
        IsPlanting = false;
    }

    #endregion

    public void onSleep()
    {
        _worldTime.SetTime(1, 59);
        transform.position = new Vector3(_bed.transform.position.x - 3, _bed.transform.position.y, _bed.transform.position.z);
    }

    public void PassOut()
    {
        transform.position = new Vector3(_bed.transform.position.x - 3, _bed.transform.position.y, _bed.transform.position.z);
    }
}
