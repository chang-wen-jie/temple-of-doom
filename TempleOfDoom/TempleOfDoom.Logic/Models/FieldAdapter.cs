using CODE_TempleOfDoom_DownloadableContent;

namespace TempleOfDoom.Logic.Models;

public class FieldAdapter(Room room, int x, int y) : IField
{
    public IField GetNeighbour(int direction)
    {
        var nextX = x;
        var nextY = y;

        switch (direction)
        {
            case 0: nextY--; break; // Up/North
            case 1: nextX++; break; // Right/East
            case 2: nextY++; break; // Down/South
            case 3: nextX--; break; // Left/West
        }
        
        // 1. Try to get the real, cached adapter from the Room
        var neighbor = room.GetField(nextX, nextY);

        // 2. If the neighbor exists (inside the map), return it.
        if (neighbor != null)
        {
            return neighbor;
        }

        return new FieldAdapter(room, nextX, nextY);
    }
    
    // FIX: logic to check if this tile is a wall.
    // room.IsWalkable returns 'false' for walls, which stops the enemy from moving there.
    public bool CanEnter => room.IsWalkable(x, y);

    // Required by IField (Not strictly needed for movement, but required by interface)
    public IPlacable Item
    {
        get => room.GetItem(x, y);
        set => room.SetItem(x, y, value);
    }
}