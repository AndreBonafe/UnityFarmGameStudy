namespace Assets.Scripts.InventorySystem
{
  public class InventorySlotChangedArgs
  {
    public ItemStack NewState { get; }
    public bool Active { get; }

    public InventorySlotChangedArgs(ItemStack newState, bool active)
    {
      NewState = newState;
      Active = active;
    }
  }
}