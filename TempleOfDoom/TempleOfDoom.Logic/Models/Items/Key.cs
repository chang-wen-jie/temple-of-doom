namespace TempleOfDoom.Logic.Models.Items;

public class Key : IItem
{
    public int XPos { get; set; }
    public int YPos { get; set; }
    public string? Color { get; init; }
    public bool IsLootable => true;

    public void Interact(Player player)
    {
        player.AddItem(this);
    }
}