using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Interfaces;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.Logic.Models.Level;
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
                ? Symbols.Wall
                : Symbols.Empty;

        return grid;
    }

    private static void AddSpecialTiles(Room room, char[,] grid)
    {
        foreach (var tile in room.SpecialFloorTiles)
        {
            if (!IsValidPosition(tile.X, tile.Y, grid)) continue;

            if (tile.Type == SpecialFloorTilesTypes.Ice) grid[tile.Y, tile.X] = Symbols.Ice;
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
            if (enemy is not Enemy baseEnemy) continue;
            var x = baseEnemy.CurrentXLocation;
            var y = baseEnemy.CurrentYLocation;

            if (IsValidPosition(x, y, grid)) 
            {
                grid[y, x] = Symbols.Enemy;
            }
        }
    }

    private static void AddPlayer(Player player, char[,] grid)
    {
        if (IsValidPosition(player.X, player.Y, grid)) grid[player.Y, player.X] = Symbols.Player;
    }

    private static char GetDoorSymbol(Door door, bool isHorizontalWall)
    {
        return door.DoorType switch
        {
            DoorTypes.Colored => isHorizontalWall ? Symbols.DoorHorizontal : Symbols.DoorVertical,
            DoorTypes.Toggle => Symbols.DoorToggle,
            DoorTypes.ClosingGate => Symbols.DoorClosingGate,
            DoorTypes.OpenOnStonesInRoom => Symbols.DoorOpenOnStonesInRoom,
            DoorTypes.OpenOnOdd => Symbols.DoorOpenOnOdd,
            DoorTypes.Ladder => Symbols.DoorLadder,
            _ => Symbols.Empty
        };
    }

    private static char GetItemSymbol(IItem item)
    {
        return item switch
        {
            SankaraStone => Symbols.SankaraStone,
            Key => Symbols.Key,
            PressurePlate => Symbols.PressurePlate,
            BoobyTrap => Symbols.BoobyTrap,
            DisappearingBoobyTrap => Symbols.DisappearingTrap,
            _ => Symbols.Empty
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