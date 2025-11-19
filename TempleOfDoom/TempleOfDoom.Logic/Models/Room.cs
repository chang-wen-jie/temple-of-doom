using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models;

public class Room
{
    public int Id { get; init; }
    public string Type { get; set; }
    public int Width { get; init; }
    public int Height { get; init; }
    public List<IItem> Items { get; init; } = [];
    public List<Door> Doors { get; } = [];
    
    public bool IsPositionValid(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        var isWall = (x == 0 || x == Width - 1 || y == 0 || y == Height - 1);

        return !isWall || Doors.Any(d => d.X == x && d.Y == y);
    }
    
    public void HandlePlayerEntry(Player player)
    {
        var item = Items.FirstOrDefault(i => i.XPos == player.StartXPos && i.YPos == player.StartYPos);

        if (item == null) return;

        item.Interact(player);

        if (ShouldRemoveItem(item))
        {
            Items.Remove(item);
        }
    }
    
    private static bool ShouldRemoveItem(IItem item)
    {
        if (item.IsLootable) return true;

        return item is DisappearingBoobyTrap { ShouldBeRemoved: true };
    }
}