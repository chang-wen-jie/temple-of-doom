using TempleOfDoom.Logic.Constants;

namespace TempleOfDoom.UI.Inputs;

public static class InputReader
{
    public static string GetDirection()
    {
        var key = Console.ReadKey(true).Key;
        return MapKeyToDirection(key);
    }

    private static string MapKeyToDirection(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.UpArrow => Direction.Up,
            ConsoleKey.DownArrow => Direction.Down,
            ConsoleKey.LeftArrow => Direction.Left,
            ConsoleKey.RightArrow => Direction.Right,
            ConsoleKey.Spacebar => Commands.Shoot,
            _ => ""
        };
    }
}