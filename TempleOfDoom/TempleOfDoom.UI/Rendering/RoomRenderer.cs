using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.UI.Rendering;

public static class RoomRenderer
{
    private const int LeftPadding = 5;

    public static void RenderRoom(Room room, Player player)
    {
        var grid = CreateRoomGrid(room);
        Console.Clear();
        
        RenderHeader();
        RenderDoors(room, grid);
        RenderItems(room, grid);
        RenderPlayer(player, grid);
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

    private static char[,] CreateRoomGrid(Room room)
    {
        var grid = new char[room.Height, room.Width];
            
        for (var y = 0; y < room.Height; y++)
        {
            for (var x = 0; x < room.Width; x++)
            {
                grid[y, x] = IsRoomBorder(x, y, room.Width, room.Height) 
                    ? '#' 
                    : ' ';
            }
        }
            
        return grid;
    }
    
    private static void RenderDoors(Room room, char[,] grid)
    {
        foreach (var door in room.Doors)
        {
            var isHorizontalWall = (door.Y == 0 || door.Y == room.Height - 1);
            
            grid[door.Y, door.X] = door.GetSymbol(isHorizontalWall);
        }
    }

    private static char GetItemSymbol(IItem item)
    {
        return item switch
        {
            SankaraStone _ => 'S',
            Key _ => 'K',
            PressurePlate _ => 'T',
            BoobyTrap _ => 'O',
            DisappearingBoobyTrap _ => '@',
            _ => '.'
        };
    }

    private static void RenderItems(Room room, char[,] grid)
    {
        foreach (var item in room.Items.Where(item => IsValidItemPosition(item, room.Width, room.Height)))
        {
            grid[item.YPos, item.XPos] = GetItemSymbol(item);
        }
    }

    private static void RenderPlayer(Player player, char[,] grid)
    {
        grid[player.StartYPos, player.StartXPos] = 'X';
    }
        
    private static void SetConsoleColor(char gridItem, Door? door, IItem? item)
    {
        if (door is { DoorType: "colored" })
        {
            Console.ForegroundColor = door.Color?.ToLower() switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                _ => ConsoleColor.Gray
            };
            return;
        }
        
        if (item is Key key)
        {
            Console.ForegroundColor = key.Color?.ToLower() switch
            {
                "red" => ConsoleColor.Red,
                "green" => ConsoleColor.Green,
                _ => ConsoleColor.Gray
            };
            return;
        }
        
        Console.ForegroundColor = gridItem switch
        {
            'X' => ConsoleColor.Blue,
            '#' => ConsoleColor.Yellow,
            'S' => ConsoleColor.DarkYellow,
            'K' => ConsoleColor.Green,
            _ => ConsoleColor.Gray
        };
    }

    private static void RenderGrid(Room room, char[,] grid)
    {
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            Console.Write(new string(' ', LeftPadding));
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                var symbol = grid[y, x];
                var door = room.Doors.FirstOrDefault(d => d.X == x && d.Y == y);
                var item = room.Items.FirstOrDefault(i => i.XPos == x && i.YPos == y);
                
                SetConsoleColor(symbol, door, item);

                Console.Write(symbol + " ");
            }
                
            Console.WriteLine();
        }
            
        Console.ResetColor();
    }
        
    private static void RenderPlayerStats(Player player)
    {
        var stones = player.GetItems().OfType<SankaraStone>().Count();

        Console.WriteLine($"\n{new string(' ', LeftPadding)}Lives: {player.Lives}");
        Console.WriteLine($"{new string(' ', LeftPadding)}Sankara Stones: {stones}/5");
        Console.WriteLine($"\n{new string(' ', LeftPadding)}Inventory:");
            
        var keys = player.GetItems().OfType<Key>();
        foreach (var key in keys)
        {
            Console.WriteLine($"{new string(' ', LeftPadding)}{key.Color} key");
        }
    }

    private static bool IsRoomBorder(int x, int y, int width, int height)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    private static bool IsValidItemPosition(IItem item, int width, int height)
    {
        return item.XPos >= 0 && item.XPos < width && 
               item.YPos >= 0 && item.YPos < height;
    }
}