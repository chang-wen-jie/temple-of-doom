using TempleOfDoom.Logic.Constants;

namespace TempleOfDoom.Logic.Helpers;

public static class DirectionHelper
{
    public static (int x, int y) GetNextPosition(int startX, int startY, string direction)
    {
        return direction switch
        {
            Direction.Up => (startX, startY - 1),
            Direction.Down => (startX, startY + 1),
            Direction.Left => (startX - 1, startY),
            Direction.Right => (startX + 1, startY),
            _ => (startX, startY)
        };
    }
}