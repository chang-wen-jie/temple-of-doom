namespace TempleOfDoom.Logic.Models.Items;

public class SankaraStone : IItem
{
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => true;

    public void Interact(Player player)
    {
        player.AddItem(this);
    }
}