using TempleOfDoom.Logic.Models.Interfaces;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Entities;

public class Player(int roomId, int x, int y, int lives)
{
    public int CurrentRoomId { get; private set; } = roomId;
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;
    public int Lives { get; private set; } = lives;

    private readonly List<IItem> _items = [];
    public IEnumerable<IItem> Inventory => _items.AsReadOnly();

    public void SetRoom(int roomId, int x, int y)
    {
        CurrentRoomId = roomId;
        SetPosition(x, y);
    }

    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void TakeDamage(int amount)
    {
        Lives = Math.Max(0, Lives - amount);
    }

    public void AddItem(IItem item)
    {
        if (item.IsLootable) _items.Add(item);
    }

    public bool HasKeyWithColor(string? requiredColor)
    {
        if (string.IsNullOrEmpty(requiredColor)) return false;

        return _items
            .OfType<Key>()
            .Any(k => k.Color != null && k.Color.Equals(requiredColor, StringComparison.OrdinalIgnoreCase));
    }
}