namespace TempleOfDoom.Logic.Models.Items;

public class Key : IItem
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public string Color { get; set; }
    public bool IsLootable => true;
    
    public void Interact(Player player)
    {
        throw new NotImplementedException();
    }
}