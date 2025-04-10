using System;
using UnityEngine;

namespace Assets.Scripts.InventorySystem
{
  [Serializable]
  public class InventorySlot
  {
    public event EventHandler<InventorySlotChangedArgs> StateChanged;
    [SerializeField] private ItemStack _state;
    private bool _isActive;

    public ItemStack State
    {
      get => _state;
      set
      {
        _state = value;
        NotifyAboutEventChanged();
      }
    }
    public bool IsActive
    {
      get => _isActive;
      set
      {
        _isActive = value;
        NotifyAboutEventChanged();
      }
    }
    public bool HasItem => _state?.Item != null;

    public bool IsTool => _state.IsTool;

    public ItemDefinition Item => _state?.Item;
    public int NumberOfItems
    {
      get => _state.NumberOfitems;
      set
      {
        _state.NumberOfitems = value;
        NotifyAboutEventChanged();
      }
    }

    public void Clear()
    {
      State = null;
    }

    private void NotifyAboutEventChanged()
    {
      StateChanged?.Invoke(this, new InventorySlotChangedArgs(_state, _isActive));
    }

    public void UpdateUIWater()
    {
      StateChanged?.Invoke(this, new InventorySlotChangedArgs(_state, _isActive));
    }
  }
}