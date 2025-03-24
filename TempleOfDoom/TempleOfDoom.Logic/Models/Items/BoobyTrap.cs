namespace TempleOfDoom.Logic.Models.Items;

public class BoobyTrap : IItem
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => false;
    
    public void Interact(Player player)
    {
        player.decreaseLives(Damage);
    }
}