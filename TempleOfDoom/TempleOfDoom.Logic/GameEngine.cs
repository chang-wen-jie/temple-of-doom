using System.Collections.Generic;
using System.Text.Json;
using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoom.Logic;

public class GameEngine
{
    static readonly List<Key> collectedKeys = new List<Key>();
    static List<IItem> collectedSankaraStones = new List<IItem>();

    public void run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "gameData.json");
        _ = new GameDataService();
            
        string jsonData = File.ReadAllText(jsonFilePath);
            
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var rootDto = JsonSerializer.Deserialize<GameDataDTO>(jsonData, options);

        try
        {
            GameDataDTO gameData = GameDataService.LoadGameData(jsonFilePath);
            Player newPlayer = GameDataService.CreatePlayer(gameData.player);

            bool gameRunning = true;
            while (gameRunning && newPlayer.lives > 0 && collectedSankaraStones.Count < 5)
            {
                Room currentRoom = gameData.rooms.FirstOrDefault(r => r.id == newPlayer.roomId);
                if (currentRoom == null)
                {
                    Console.WriteLine($"Invalid room ID: {newPlayer.roomId}");
                    break;
                }

                DisplayRoom(currentRoom, gameData.connections, newPlayer);

                var key = Console.ReadKey(true).Key;
                string direction = "";

                direction = key switch
                {
                    ConsoleKey.UpArrow => "up",
                    ConsoleKey.DownArrow => "down",
                    ConsoleKey.LeftArrow => "left",
                    ConsoleKey.RightArrow => "right",
                    _ => direction
                };

                if (!string.IsNullOrEmpty(direction))
                {
                    int previousX = newPlayer.xPosition;
                    int previousY = newPlayer.yPosition;
                    MovePlayerInRoom(newPlayer, currentRoom, direction);

                    if (IsAtDoor(newPlayer, currentRoom, direction, gameData.connections))
                    {
                        if (!MoveToNextRoom(newPlayer, direction, gameData.connections, gameData.rooms))
                        {
                            newPlayer.xPosition = previousX;
                            newPlayer.yPosition = previousY;
                        }
                    }
                }
            }
            if (collectedSankaraStones.Count == 5)
            {
                Console.WriteLine(new string('*', 50) + "\n" + new string(' ', 5) + "YOU WON\n" + new string('*', 50));
            }
            else if (newPlayer.lives >= 0)
            {
                Console.WriteLine(new string('*', 50) + "\n" + new string(' ', 5) + "GAME OVER\n" + new string('*', 50));

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Program.cs Error: {ex.Message}");
        }
    }

    static void DisplayRoom(Room room, ConnectionDto[] connections, Player player)
    {
        int leftPadding = 5;
        Console.Clear();
        Console.WriteLine("\n" + new string(' ', leftPadding) + "Welcome to the Temple of Doom!\n" + new string('-', 50) + "\n" + new string('-', 50) + "\n\n\n");

        // Kamer in een rooster opslaan
        char[,] grid = new char[room.height, room.width];
        for (int y = 0; y < room.height; y++)
        {
            for (int x = 0; x < room.width; x++)
            {
                if (x == 0 || y == 0 || x == room.width - 1 || y == room.height - 1) grid[y, x] = '#';
                else grid[y, x] = ' ';
            }
        }

        foreach (var item in room.items ?? Enumerable.Empty<IItem>())
        {
            if (item != null && item.x >= 0 && item.x < room.width && item.y >= 0 && item.y < room.height)
            {
                grid[item.y, item.x] = item switch
                {
                    SankaraStone => 'S',
                    Key key => 'K',
                    PressurePlate => 'T',
                    BoobyTrap => 'O',
                    DisappearingBoobyTrap => '@',
                    _ => '.',
                };
            }
        }

        if (player.roomId == room.Id) grid[player.yPosition, player.xPosition] = 'X';

        foreach (var connection in connections)
        {
            switch (room.Id)
            {
                case var id when id == connection.SOUTH:
                    PlaceDoors(grid, connection.doors, 0, room.width, "horizontal");
                    break;
                case var id when id == connection.NORTH:
                    PlaceDoors(grid, connection.doors, room.height - 1, room.width, "horizontal");
                    break;
                case var id when id == connection.EAST:
                    PlaceDoors(grid, connection.doors, 0, room.height, "vertical");
                    break;
                case var id when id == connection.WEST:
                    PlaceDoors(grid, connection.doors, room.width - 1, room.height, "vertical");
                    break;
            }
        }

        // 2D array uitprinten
        for (int y = 0; y < room.height; y++)
        {
            Console.Write(new string(' ', leftPadding));
            for (int x = 0; x < room.width; x++)
            {
                if (grid[y, x] == '#')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if ((y == 0 || y == room.height - 1 || x == 0 || x == room.width - 1) && (grid[y, x] == '=' || grid[y, x] == '|'))
                {
                    var door = connections.SelectMany(c => c.doors).FirstOrDefault(d => d.type == "colored");
                    Console.ForegroundColor = door?.color switch
                    {
                        "red" => ConsoleColor.Red,
                        "green" => ConsoleColor.Green,
                        _ => ConsoleColor.Gray
                    };
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                var item = room.items?.FirstOrDefault(i => i.x == x && i.y == y);

                if (item != null)
                {
                    Console.ForegroundColor = item switch
                    {
                        SankaraStone => ConsoleColor.DarkYellow,
                        Key key => key.Color switch
                        {
                            Enums.Color.Red => ConsoleColor.Red,
                            Enums.Color.Green => ConsoleColor.Green,
                            _ => ConsoleColor.Gray,
                        },
                        _ => ConsoleColor.Gray,
                    };
                }
                Console.Write(grid[y, x] + " ");
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("\n\n" + new string(' ', leftPadding) + "Lives: " + player.lives + "\n" + new string(' ', leftPadding) + "Stones: " + collectedSankaraStones.Count);
        Console.WriteLine("\n\n\n" + new string('-', 50) + "\n" + new string('-', 50) + "\n" + new string(' ', leftPadding) + "Inventory");
        foreach (var key in collectedKeys)
        {
            Console.WriteLine(new string(' ', leftPadding) + key.Color + " key");
        }
        Console.WriteLine(new string('-', 50) + "\n" + new string('-', 50));
    }

    static void PlaceDoors(char[,] grid, IDoor[] doors, int fixedCoord, int variableLimit, string direction)
    {
        int middle = variableLimit / 2;

        char doorSymbol = doors?.FirstOrDefault()?.Type switch
        {
            "colored" => direction == "horizontal" ? '=' : '|',
            "toggle" => '⊥',
            "closing gate" => '∩',
            "open on stones in room" => '?',
            "open on odd" => '!',
            _ => ' '
        };

        if (direction == "horizontal")
        {
            grid[fixedCoord, middle] = doorSymbol;
        }
        else if (direction == "vertical")
        {
            grid[middle, fixedCoord] = doorSymbol;
        }
    }

    static void MovePlayerInRoom(Player player, Room currentRoom, string direction)
    {
        int newX = player.xPosition;
        int newY = player.yPosition;

        switch (direction)
        {
            case "up": newY--; break;
            case "down": newY++; break;
            case "left": newX--; break;
            case "right": newX++; break;
        }

        if (newX >= 0 && newX < currentRoom.width && newY >= 0 && newY < currentRoom.height)
        {
            player.xPosition = newX;
            player.yPosition = newY;
            HandleItemInteraction(player, currentRoom);
        }
    }

    static void HandleItemInteraction(Player player, Room currentRoom)
    {
        var item = currentRoom.items?.FirstOrDefault(i => i.x == player.xPosition && i.y == player.yPosition);
        if (item != null)
        {
            switch (item)
            {
                case BoobyTrap _:
                case DisappearingBoobyTrap _:
                    player.SubtractLives(1);
                    if (item is DisappearingBoobyTrap) RemoveItemFromRoom(currentRoom, item);
                    break;
                case Key key:
                    collectedKeys.Add(key);
                    RemoveItemFromRoom(currentRoom, key);
                    break;
                case SankaraStone stone:
                    collectedSankaraStones.Add(stone);
                    RemoveItemFromRoom(currentRoom, stone);
                    break;

            }
        }
    }

    static void RemoveItemFromRoom(Room room, IItem item)
    {
        room.items.Remove(item);
    }

    static bool IsAtDoor(Player player, Room currentRoom, string direction, ConnectionDto[] connections)
    {
        bool atEdge = (direction == "up" && player.yPosition == 0) ||
                      (direction == "down" && player.yPosition == currentRoom.height - 1) ||
                      (direction == "left" && player.xPosition == 0) ||
                      (direction == "right" && player.xPosition == currentRoom.width - 1);

        if (!atEdge) return false;

        var relevantConnection = connections.FirstOrDefault(c =>
            (direction == "up" && c.SOUTH == currentRoom.Id) ||
            (direction == "down" && c.NORTH == currentRoom.Id) ||
            (direction == "left" && c.EAST == currentRoom.Id) ||
            (direction == "right" && c.WEST == currentRoom.Id));

        if (relevantConnection != null)
        {
            var coloredDoor = relevantConnection.doors.FirstOrDefault(d => d.type == "colored");
            if (coloredDoor != null)
            {
                bool hasMatchingKey = collectedKeys.Any(k => k.Color == coloredDoor.color);
                if (!hasMatchingKey) return false;
            }

            var closingGate = relevantConnection.doors.FirstOrDefault(d => d.type == "closing gate");
            if (closingGate != null && closingGate.isClosedPermanently) return false;
        }

        return relevantConnection != null;
    }

    static bool MoveToNextRoom(Player player, string direction, ConnectionDto[] connections, Room[] rooms)
    {
        var relevantConnections = connections.Where(c =>
            c.NORTH == player.roomId || c.SOUTH == player.roomId ||
            c.WEST == player.roomId || c.EAST == player.roomId).ToList();

        foreach (var connection in relevantConnections)
        {
            var coloredDoor = connection.doors.FirstOrDefault(d => d.type == "colored");
            if (coloredDoor != null && !collectedKeys.Any(k => k.Color == coloredDoor.color)) return false;

            Room currentRoom = rooms.First(r => r.Id == player.roomId);
            Room nextRoom;
            switch (direction)
            {
                case "up" when connection.SOUTH == player.roomId:
                    nextRoom = rooms.First(r => r.Id == connection.NORTH);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    player.roomId = connection.NORTH;
                    player.yPosition = nextRoom.height - 1;
                    player.xPosition = (player.xPosition - (currentRoom.width / 2)) + (nextRoom.width / 2);
                    break;
                case "down" when connection.NORTH == player.roomId:
                    nextRoom = rooms.First(r => r.Id == connection.SOUTH);
                    MarkDoorAsClosed(connection.doors, "closing gate");

                    player.roomId = connection.SOUTH;
                    player.yPosition = 0;
                    player.xPosition = (player.xPosition - (currentRoom.width / 2)) + (nextRoom.width / 2);
                    break;
                case "left" when connection.EAST == player.roomId:
                    nextRoom = rooms.First(r => r.Id == connection.WEST);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    player.roomId = connection.WEST;
                    player.xPosition = nextRoom.width - 1;
                    player.yPosition = (player.yPosition - (currentRoom.height / 2)) + (nextRoom.height / 2);
                    break;
                case "right" when connection.WEST == player.roomId:
                    nextRoom = rooms.First(r => r.Id == connection.EAST);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    player.roomId = connection.EAST;
                    player.xPosition = 0;
                    player.yPosition = (player.yPosition - (currentRoom.height / 2)) + (nextRoom.height / 2);
                    break;
                default:
                    continue;
            }

            // Voorkom dat speler buiten de grenzen van de volgende kamer komt
            player.xPosition = Math.Max(0, Math.Min(player.xPosition, nextRoom.width - 1));
            player.yPosition = Math.Max(0, Math.Min(player.yPosition, nextRoom.height - 1));
            return true;
        }

        return false;
    }

    // voor de closing gate
    static void MarkDoorAsClosed(IDoor[] doors, string doorType)
    {
        var doorToClose = doors.FirstOrDefault(d => d.Type == doorType);
        if (doorToClose != null)
        {
            doorToClose.isClosedPermanently = true; // Set the door's state to closed permanently
        }
    }
}