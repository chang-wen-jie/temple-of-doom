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
            case 0: nextY--; break;
            case 1: nextX++; break;
            case 2: nextY++; break;
            case 3: nextX--; break;
        }

        var neighbor = room.GetField(nextX, nextY);

        return neighbor ?? new FieldAdapter(room, nextX, nextY);
    }

    public bool CanEnter => room.IsWalkable(x, y);

    public IPlacable? Item
    {
        get => room.GetItem(x, y);
        set => room.SetItem(x, y, value);
    }
}