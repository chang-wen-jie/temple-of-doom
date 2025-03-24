namespace TempleOfDoom.Logic.Models.Items;

public interface IItem
{
    string Type { get; set; }
    int Damage { get; set; }
    int XPos { get; set; }
    int YPos { get; set; }
    bool IsLootable { get; }
    
    void Interact(Player player);
}