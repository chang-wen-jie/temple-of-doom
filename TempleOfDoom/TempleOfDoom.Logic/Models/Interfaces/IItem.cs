using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Interfaces;

public interface IItem
{
    int XPos { get; }
    int YPos { get; }
    bool IsLootable { get; }

    event EventHandler? OnItemDepleted;

    void Interact(Player player);
}