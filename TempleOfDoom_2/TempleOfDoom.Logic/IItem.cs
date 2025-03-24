namespace TempleOfDoom.Logic;

public interface IItem
{
    int x { get; set; }
    int y { get; set; }
    string Type { get; set; }
    bool IsLootable { get; set; }

    void Interact();
}