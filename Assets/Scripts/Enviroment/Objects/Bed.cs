using Assembly_CSharp.Assets.Scripts.WorldTime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _sleepDialog;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private GameObject inventoryUI; 
    [SerializeField] private WorldTime _worldTime;
    [SerializeField] private EventSystem _eventSystem;

    private void Awake() {
        _sleepDialog.SetActive(false);
        _yesButton.onClick.AddListener(Sleep);
        _noButton.onClick.AddListener(TurnOffSleepDialog);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            _eventSystem.SetSelectedGameObject(_yesButton.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            _worldTime.Stop();
            _player.IsInBed = true;
            _sleepDialog.SetActive(true);
            inventoryUI.SetActive(false);
            _eventSystem.SetSelectedGameObject(_yesButton.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TurnOffSleepDialog();
        }
    }

    public void TurnOffSleepDialog()
    {
        _sleepDialog.SetActive(false);
        _worldTime.Resume();
        _player.IsInBed = false;
    }

    private void Sleep()
    {
        _player.onSleep();
        TurnOffSleepDialog();
    }
}
