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
            ConsoleKey.UpArrow => "up",
            ConsoleKey.DownArrow => "down",
            ConsoleKey.LeftArrow => "left",
            ConsoleKey.RightArrow => "right",
            ConsoleKey.Spacebar => "shoot",
            _ => ""
        };
    }
}