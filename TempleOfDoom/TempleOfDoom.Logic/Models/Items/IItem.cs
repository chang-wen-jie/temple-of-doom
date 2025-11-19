namespace TempleOfDoom.Logic.Models.Items;

public interface IItem
{
    int XPos { get; set; }
    int YPos { get; set; }
    bool IsLootable { get; }
    
    void Interact(Player player);
}