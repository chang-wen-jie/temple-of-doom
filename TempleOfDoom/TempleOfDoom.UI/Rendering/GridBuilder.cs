using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.UI.Constants;

namespace TempleOfDoom.UI.Rendering;

public static class GridBuilder
{
    public static char[,] Build(Room room, Player player)
    {
        var grid = InitializeGrid(room);

        AddSpecialTiles(room, grid);
        AddDoors(room, grid);
        AddItems(room, grid);
        AddEnemies(room, grid);
        AddPlayer(player, grid);

        return grid;
    }

    private static char[,] InitializeGrid(Room room)
    {
        var grid = new char[room.Height, room.Width];

        for (var y = 0; y < room.Height; y++)
        for (var x = 0; x < room.Width; x++)
            grid[y, x] = IsRoomBorder(x, y, room.Width, room.Height)
                ? ConsoleSymbols.Wall
                : ConsoleSymbols.Empty;

        return grid;
    }

    private static void AddSpecialTiles(Room room, char[,] grid)
    {
        foreach (var tile in room.SpecialFloorTiles)
        {
            if (!IsValidPosition(tile.X, tile.Y, grid)) continue;

            if (tile.Type == SpecialFloorTilesTypes.Ice) grid[tile.Y, tile.X] = ConsoleSymbols.Ice;
        }
    }

    private static void AddDoors(Room room, char[,] grid)
    {
        foreach (var door in room.Doors)
        {
            var isHorizontal = door.Y == 0 || door.Y == room.Height - 1;
            grid[door.Y, door.X] = GetDoorSymbol(door, isHorizontal);
        }
    }

    private static void AddItems(Room room, char[,] grid)
    {
        foreach (var item in room.Items)
            if (IsValidPosition(item.XPos, item.YPos, grid))
                grid[item.YPos, item.XPos] = GetItemSymbol(item);
    }

    private static void AddEnemies(Room room, char[,] grid)
    {
        foreach (var enemy in room.Enemies)
        {
            var (x, y) = enemy switch
            {
                HorizontallyMovingEnemy h => (h.CurrentXLocation, h.CurrentYLocation),
                VerticallyMovingEnemy v => (v.CurrentXLocation, v.CurrentYLocation),
                _ => (-1, -1)
            };

            if (IsValidPosition(x, y, grid)) grid[y, x] = ConsoleSymbols.Enemy;
        }
    }

    private static void AddPlayer(Player player, char[,] grid)
    {
        if (IsValidPosition(player.X, player.Y, grid)) grid[player.Y, player.X] = ConsoleSymbols.Player;
    }

    private static char GetDoorSymbol(Door door, bool isHorizontalWall)
    {
        return door.DoorType switch
        {
            DoorTypes.Colored => isHorizontalWall ? ConsoleSymbols.DoorHorizontal : ConsoleSymbols.DoorVertical,
            DoorTypes.Toggle => ConsoleSymbols.DoorToggle,
            DoorTypes.ClosingGate => ConsoleSymbols.DoorClosingGate,
            DoorTypes.OpenOnStonesInRoom => ConsoleSymbols.DoorOpenOnStonesInRoom,
            DoorTypes.OpenOnOdd => ConsoleSymbols.DoorOpenOnOdd,
            DoorTypes.Ladder => ConsoleSymbols.DoorLadder,
            _ => ConsoleSymbols.Empty
        };
    }

    private static char GetItemSymbol(IItem item)
    {
        return item switch
        {
            SankaraStone => ConsoleSymbols.SankaraStone,
            Key => ConsoleSymbols.Key,
            PressurePlate => ConsoleSymbols.PressurePlate,
            BoobyTrap => ConsoleSymbols.BoobyTrap,
            DisappearingBoobyTrap => ConsoleSymbols.DisappearingTrap,
            _ => '.'
        };
    }

    private static bool IsRoomBorder(int x, int y, int width, int height)
    {
        return x == 0 || y == 0 || x == width - 1 || y == height - 1;
    }

    private static bool IsValidPosition(int x, int y, char[,] grid)
    {
        return x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0);
    }
}