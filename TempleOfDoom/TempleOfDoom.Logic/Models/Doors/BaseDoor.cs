using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Doors;

public class BaseDoor : Door
{
    public bool IsOpen { get; private set; } = true;

    public bool CanEnter(Player player)
    {
        if (DoorType == "colored" && !string.IsNullOrEmpty(Color))
        {
            bool hasKey = player.GetItems()
                .OfType<Key>() // Ensure you are using System.Linq
                .Any(k => k.Color.Equals(Color, StringComparison.OrdinalIgnoreCase));

            return hasKey;
        }
        
        // 2. Logic for Closing Gates (One-way)
        // If a gate is "closed", you can't enter. 
        // Note: Your old code closes it AFTER entry, which is handled in the interaction step.
        if (DoorType == "closing gate" && !IsOpen)
        {
            return false;
        }

        // 3. Default
        return true;
    }
    
    public void Interact()
    {
        if (DoorType == "closing gate")
        {
            Close(); // Permanently close behind you
        }
    }

    public override void Open() => IsOpen = true;
    public override void Close() => IsOpen = false;
}