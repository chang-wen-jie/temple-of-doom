using TempleOfDoom.Logic.Constants;
using TempleOfDoom.UI.Constants;

namespace TempleOfDoom.UI.Rendering;

public static class ConsoleColorMapper
{
    private static readonly Dictionary<string, ConsoleColor> NamedColorMap = new()
    {
        { Colors.Red, ConsoleColor.Red },
        { Colors.Green, ConsoleColor.Green }
    };

    private static readonly Dictionary<char, ConsoleColor> SymbolColorMap = new()
    {
        { Symbols.Player, ConsoleColor.Blue },
        { Symbols.Wall, ConsoleColor.Yellow },
        { Symbols.SankaraStone, ConsoleColor.DarkYellow },
        { Symbols.Key, ConsoleColor.Green },
        { Symbols.Enemy, ConsoleColor.Red },
        { Symbols.Ice, ConsoleColor.Cyan },
        { Symbols.DoorLadder, ConsoleColor.Magenta }
    };

    public static ConsoleColor GetColor(string? colorName)
    {
        return string.IsNullOrEmpty(colorName)
            ? ConsoleColor.Gray
            : NamedColorMap.GetValueOrDefault(colorName.ToLower(), ConsoleColor.Gray);
    }

    public static ConsoleColor GetColor(char symbol)
    {
        return SymbolColorMap.GetValueOrDefault(symbol, ConsoleColor.Gray);
    }
}