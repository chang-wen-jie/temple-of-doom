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
    private const int LeftPadding = 5;

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
            new string(' ', LeftPadding) + "Welcome to the Temple of Doom!\n" +
            new string('-', 50) + "\n"
        );
    }

    // Kamerobjecten bekleuren
    private static void RenderGrid(Room room, char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            Console.Write(new string(' ', LeftPadding));
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                var symbol = grid[y, x];
                // Objecten ophalen om kleuren te constateren
                var door = room.Doors.FirstOrDefault(d => d.X == x && d.Y == y);
                var item = room.Items.FirstOrDefault(i => i.XPos == x && i.YPos == y);

                SetConsoleColor(symbol, door, item);
                Console.Write(symbol + " ");
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    private static void SetConsoleColor(char symbol, Door? door, IItem? item)
    {
        if (door is { DoorType: DoorTypes.Colored })
        {
            Console.ForegroundColor = door.Color?.ToLower() switch
            {
                GameColors.Red => ConsoleColor.Red,
                GameColors.Green => ConsoleColor.Green,
                _ => ConsoleColor.Gray
            };
            return;
        }

        if (item is Key key)
        {
            Console.ForegroundColor = key.Color?.ToLower() switch
            {
                GameColors.Red => ConsoleColor.Red,
                GameColors.Green => ConsoleColor.Green,
                _ => ConsoleColor.Gray
            };
            return;
        }

        Console.ForegroundColor = symbol switch
        {
            ConsoleSymbols.Player => ConsoleColor.Blue,
            ConsoleSymbols.Wall => ConsoleColor.Yellow,
            ConsoleSymbols.SankaraStone => ConsoleColor.DarkYellow,
            ConsoleSymbols.Key => ConsoleColor.Green,
            ConsoleSymbols.Enemy => ConsoleColor.Red,
            ConsoleSymbols.Ice => ConsoleColor.Cyan,
            ConsoleSymbols.DoorLadder => ConsoleColor.Magenta,
            _ => ConsoleColor.Gray
        };
    }

    private static void RenderPlayerStats(Player player)
    {
        var stones = player.Inventory.OfType<SankaraStone>().Count();

        Console.WriteLine($"\n{new string(' ', LeftPadding)}Lives: {player.Lives}");
        Console.WriteLine($"{new string(' ', LeftPadding)}Sankara Stones: {stones}/{GameRules.WinningStoneCount}");
        Console.WriteLine($"\n{new string(' ', LeftPadding)}Inventory:");

        var keys = player.Inventory.OfType<Key>();
        foreach (var key in keys) Console.WriteLine($"{new string(' ', LeftPadding)}{key.Color} key");
    }

    public static void RenderMessage(string message)
    {
        Console.WriteLine("\n" + new string('*', 50));
        Console.WriteLine(new string(' ', LeftPadding) + message);
        Console.WriteLine(new string('*', 50));
    }
}