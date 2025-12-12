using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Interfaces;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.Logic.Models.Level;
using TempleOfDoom.UI.Constants;

namespace TempleOfDoom.UI.Rendering;

public static class RoomRenderer
{
    public static void RenderRoom(Room room, Player player)
    {
        var grid = GridBuilder.Build(room, player);

        Console.Clear();
        RenderHeader();
        RenderGrid(room, grid);
        RenderPlayerStats(player);
    }

    private static void RenderHeader()
    {
        Console.WriteLine(
            "\n" +
            new string(Symbols.Empty, Spacing.LeftPadding) + "Welcome to the Temple of Doom!\n" +
            new string(Symbols.Dash, Spacing.SeperatorLength) + "\n"
        );
    }

    private static void RenderGrid(Room room, char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            Console.Write(new string(Symbols.Empty, Spacing.LeftPadding));
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                var symbol = grid[y, x];
                // Objecten ophalen om kleuren te constateren
                var door = room.Doors.FirstOrDefault(d => d.X == x && d.Y == y);
                var item = room.Items.FirstOrDefault(i => i.XPos == x && i.YPos == y);

                SetConsoleColor(symbol, door, item);
                Console.Write($"{symbol}{Symbols.Empty}");
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    private static void SetConsoleColor(char symbol, Door? door, IItem? item)
    {
        if (door is { DoorType: DoorTypes.Colored })
        {
            Console.ForegroundColor = ConsoleColorMapper.GetColor(door.Color);
            return;
        }

        if (item is Key key && !string.IsNullOrEmpty(key.Color))
        {
            Console.ForegroundColor = ConsoleColorMapper.GetColor(key.Color);
            return;
        }

        Console.ForegroundColor = ConsoleColorMapper.GetColor(symbol);
    }

    private static void RenderPlayerStats(Player player)
    {
        var stones = player.Inventory.OfType<SankaraStone>().Count();

        Console.WriteLine($"\n{new string(Symbols.Empty, Spacing.LeftPadding)}Lives: {player.Lives}");
        Console.WriteLine($"{new string(Symbols.Empty, Spacing.LeftPadding)}Sankara Stones: {stones}/{Rules.WinningStoneCount}");
        Console.WriteLine($"\n{new string(Symbols.Empty, Spacing.LeftPadding)}Inventory:");

        var keys = player.Inventory.OfType<Key>();
        foreach (var key in keys) Console.WriteLine($"{new string(Symbols.Empty, Spacing.LeftPadding)}{key.Color} key");
    }

    public static void RenderMessage(string message)
    {
        Console.WriteLine("\n" + new string(Symbols.Asterik, Spacing.SeperatorLength));
        Console.WriteLine(new string(Symbols.Empty, Spacing.LeftPadding) + message);
        Console.WriteLine(new string(Symbols.Asterik, Spacing.SeperatorLength));
    }
}