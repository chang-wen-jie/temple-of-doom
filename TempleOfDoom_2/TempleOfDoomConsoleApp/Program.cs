using System.Numerics;
using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoomConsoleApp
{
    class Program
    {
        static readonly List<Item> collectedKeys = new List<Item>();
        static List<Item> collectedSankaraStones = new List<Item>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // Deursymbolen uitprintbaar maken
            string jsonFilePath = @"C:\Users\Chang Wen Jie\source\repos\deelopdracht-1-24-25-temple-of-doom-mohammedbogatyrev_wenjiechang\TempleOfDoom\TempleOfDoom.Data\Resources\gameData.json";
            _ = new GameDataService();

            try
            {
                GameDataDTO gameData = GameDataService.LoadGameData(jsonFilePath);
                Player newPlayer = GameDataService.CreatePlayer(gameData.player);

                bool gameRunning = true;
                while (gameRunning && newPlayer.lives > 0 && collectedSankaraStones.Count < 5)
                {
                    Room currentRoom = gameData.rooms.FirstOrDefault(r => r.id == newPlayer.startRoomId);
                    if (currentRoom == null)
                    {
                        Console.WriteLine($"Invalid room ID: {newPlayer.startRoomId}");
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
                        int previousX = newPlayer.startX;
                        int previousY = newPlayer.startY;
                        MovePlayerInRoom(newPlayer, currentRoom, direction);

                        if (IsAtDoor(newPlayer, currentRoom, direction, gameData.connections))
                        {
                            if (!MoveToNextRoom(newPlayer, direction, gameData.connections, gameData.rooms))
                            {
                                newPlayer.startX = previousX;
                                newPlayer.startY = previousY;
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

        static void DisplayRoom(Room room, Connection[] connections, Player player)
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

            foreach (var item in room.items ?? Enumerable.Empty<Item>())
            {
                if (item != null && item.x >= 0 && item.x < room.width && item.y >= 0 && item.y < room.height)
                {
                    grid[item.y, item.x] = item.type switch
                    {
                        "sankara stone" => 'S',
                        "key" => 'K',
                        "pressure plate" => 'T',
                        "boobytrap" => 'O',
                        "disappearing boobytrap" => '@',
                        _ => '.',
                    };
                }
            }

            if (player.startRoomId == room.id) grid[player.startY, player.startX] = 'X';

            foreach (var connection in connections)
            {
                switch (room.id)
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
                        Console.ForegroundColor = item.type switch
                        {
                            "sankara stone" => ConsoleColor.DarkYellow,
                            "key" => item.color switch
                            {
                                "red" => ConsoleColor.Red,
                                "green" => ConsoleColor.Green,
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
                Console.WriteLine(new string(' ', leftPadding) + key.color + " key");
            }
            Console.WriteLine(new string('-', 50) + "\n" + new string('-', 50));
        }

        static void PlaceDoors(char[,] grid, Door[] doors, int fixedCoord, int variableLimit, string direction)
        {
            int middle = variableLimit / 2;

            char doorSymbol = doors?.FirstOrDefault()?.type switch
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
            int newX = player.startX;
            int newY = player.startY;

            switch (direction)
            {
                case "up": newY--; break;
                case "down": newY++; break;
                case "left": newX--; break;
                case "right": newX++; break;
            }

            if (newX >= 0 && newX < currentRoom.width && newY >= 0 && newY < currentRoom.height)
            {
                player.startX = newX;
                player.startY = newY;
                HandleItemInteraction(player, currentRoom);
            }
        }

        static void HandleItemInteraction(Player player, Room currentRoom)
        {
            var item = currentRoom.items?.FirstOrDefault(i => i.x == player.startX && i.y == player.startY);
            if (item != null)
            {
                switch (item.type)
                {
                    case "boobytrap":
                    case "disappearing boobytrap":
                        player.lives--;
                        if (item.type == "disappearing boobytrap") RemoveItemFromRoom(currentRoom, item);
                        break;
                    case "key":
                        collectedKeys.Add(item);
                        RemoveItemFromRoom(currentRoom, item);
                        break;
                    case "sankara stone":
                        collectedSankaraStones.Add(item);
                        RemoveItemFromRoom(currentRoom, item);
                        break;

                }
            }
        }

        static void RemoveItemFromRoom(Room room, Item item)
        {
            room.items = room.items.Where(i => i != item).ToArray();
        }

        static bool IsAtDoor(Player player, Room currentRoom, string direction, Connection[] connections)
        {
            bool atEdge = (direction == "up" && player.startY == 0) ||
                          (direction == "down" && player.startY == currentRoom.height - 1) ||
                          (direction == "left" && player.startX == 0) ||
                          (direction == "right" && player.startX == currentRoom.width - 1);

            if (!atEdge) return false;

            var relevantConnection = connections.FirstOrDefault(c =>
                (direction == "up" && c.SOUTH == currentRoom.id) ||
                (direction == "down" && c.NORTH == currentRoom.id) ||
                (direction == "left" && c.EAST == currentRoom.id) ||
                (direction == "right" && c.WEST == currentRoom.id));

            if (relevantConnection != null)
            {
                var coloredDoor = relevantConnection.doors.FirstOrDefault(d => d.type == "colored");
                if (coloredDoor != null)
                {
                    bool hasMatchingKey = collectedKeys.Any(k => k.color == coloredDoor.color);
                    if (!hasMatchingKey) return false;
                }

                var closingGate = relevantConnection.doors.FirstOrDefault(d => d.type == "closing gate");
                if (closingGate != null && closingGate.isClosedPermanently) return false;
            }

            return relevantConnection != null;
        }

        static bool MoveToNextRoom(Player player, string direction, Connection[] connections, Room[] rooms)
        {
            var relevantConnections = connections.Where(c =>
                c.NORTH == player.startRoomId || c.SOUTH == player.startRoomId ||
                c.WEST == player.startRoomId || c.EAST == player.startRoomId).ToList();

            foreach (var connection in relevantConnections)
            {
                var coloredDoor = connection.doors.FirstOrDefault(d => d.type == "colored");
                if (coloredDoor != null && !collectedKeys.Any(k => k.color == coloredDoor.color)) return false;

                Room currentRoom = rooms.First(r => r.id == player.startRoomId);
                Room nextRoom;
                switch (direction)
                {
                    case "up" when connection.SOUTH == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.NORTH);
                        MarkDoorAsClosed(connection.doors, "closing gate"); // Close the gate permanently
                        player.startRoomId = connection.NORTH;
                        player.startY = nextRoom.height - 1;
                        player.startX = (player.startX - (currentRoom.width / 2)) + (nextRoom.width / 2);
                        break;
                    case "down" when connection.NORTH == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.SOUTH);
                        MarkDoorAsClosed(connection.doors, "closing gate"); // Close the gate permanently

                        player.startRoomId = connection.SOUTH;
                        player.startY = 0;
                        player.startX = (player.startX - (currentRoom.width / 2)) + (nextRoom.width / 2);
                        break;
                    case "left" when connection.EAST == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.WEST);
                        MarkDoorAsClosed(connection.doors, "closing gate"); // Close the gate permanently
                        player.startRoomId = connection.WEST;
                        player.startX = nextRoom.width - 1;
                        player.startY = (player.startY - (currentRoom.height / 2)) + (nextRoom.height / 2);
                        break;
                    case "right" when connection.WEST == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.EAST);
                        MarkDoorAsClosed(connection.doors, "closing gate"); // Close the gate permanently
                        player.startRoomId = connection.EAST;
                        player.startX = 0;
                        player.startY = (player.startY - (currentRoom.height / 2)) + (nextRoom.height / 2);
                        break;
                    default:
                        continue;
                }

                // Voorkom dat speler buiten de grenzen van de volgende kamer komt
                player.startX = Math.Max(0, Math.Min(player.startX, nextRoom.width - 1));
                player.startY = Math.Max(0, Math.Min(player.startY, nextRoom.height - 1));
                return true;
            }

            return false;
        }

        static void MarkDoorAsClosed(Door[] doors, string doorType)
        {
            var doorToClose = doors.FirstOrDefault(d => d.type == doorType);
            if (doorToClose != null)
            {
                doorToClose.isClosedPermanently = true; // Set the door's state to closed permanently
            }
        }
    }
}
