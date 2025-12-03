namespace TempleOfDoom.Logic.Models.Items;

public class BoobyTrap : IItem
{
    public int Damage { get; init; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => false;
    
    public void Interact(Player player)
    {
        player.TakeDamage(Damage);
    }
}