namespace TempleOfDoom.Logic.Models.Items;

public class DisappearingBoobyTrap : IItem
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => false;
    
    // TODO: Use Observer pattern to Notify the game that the item should be removed instead of current implementation
    public bool ShouldBeRemoved { get; private set; }
    
    public void Interact(Player player)
    {
        player.DecreaseLives(Damage);
        ShouldBeRemoved = true;
    }
}