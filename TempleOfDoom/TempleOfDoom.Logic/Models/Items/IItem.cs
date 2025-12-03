namespace TempleOfDoom.Logic.Models.Items;

public interface IItem
{
    int XPos { get; }
    int YPos { get; }
    bool IsLootable { get; }

    void Interact(Player player);
}