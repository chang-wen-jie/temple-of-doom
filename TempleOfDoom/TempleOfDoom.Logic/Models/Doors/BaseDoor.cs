using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Doors;

public class BaseDoor : Door
{
    public bool IsOpen { get; set; } = true;
    public Door? TwinDoor { get; set; }

    public bool CanEnter(Player player)
    {
        switch (DoorType)
        {
            case "colored" when !string.IsNullOrEmpty(Color):
            {
                var hasKey = player.GetItems()
                    .OfType<Key>()
                    .Any(k => k.Color.Equals(Color, StringComparison.OrdinalIgnoreCase));

                return hasKey;
            }
            // 2. Logic for Closing Gates (One-way)
            // If a gate is "closed", you can't enter. 
            // Note: Your old code closes it AFTER entry, which is handled in the interaction step.
            case "closing gate" when !IsOpen:
                return false;
            default:
                return true;
        }
    }
    
    public void Interact()
    {
        if (DoorType == "closing gate")
        {
            Close();
        }
    }

    public override void Open()
    {
        if (IsOpen) return; 

        IsOpen = true;
    
        (TwinDoor as BaseDoor)?.Open(); 
    }
    
    public override void Close()
    {
        if (!IsOpen) return; 

        IsOpen = false;
    
        (TwinDoor as BaseDoor)?.Close();
    }
}