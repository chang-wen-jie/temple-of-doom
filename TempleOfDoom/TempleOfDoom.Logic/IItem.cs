namespace TempleOfDoom.Logic;

public interface IItem
{
    bool IsLootable { get; set; }

    void Interact();
}