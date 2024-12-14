using System.Numerics;
using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoomConsoleApp
{
    class Program
    {
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
                while (gameRunning && newPlayer.lives > 0)
                {
                    Room currentRoom = gameData.rooms.FirstOrDefault(r => r.id == newPlayer.startRoomId);
                    if (currentRoom == null)
                    {
                        Console.WriteLine($"Invalid room ID: {newPlayer.startRoomId}");
                        break;
                    }

                    DisplayRoom(currentRoom, gameData.connections, newPlayer);
                    Console.WriteLine(new string(' ', 5) + "Use arrow keys to move, 'Q' to quit:\n" + new string('-', 50) + "\n" + new string('-', 50));

                    var key = Console.ReadKey(true).Key;
                    string direction = "";

                    switch (key)
                    {
                        case ConsoleKey.UpArrow: direction = "up"; break;
                        case ConsoleKey.DownArrow: direction = "down"; break;
                        case ConsoleKey.LeftArrow: direction = "left"; break;
                        case ConsoleKey.RightArrow: direction = "right"; break;
                        case ConsoleKey.Q: gameRunning = false; break;
                    }

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

                if (newPlayer.lives <= 0)
                {
                    Console.WriteLine("Game Over! You have no lives left.");
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
                    if (x == 0 || y == 0 || x == room.width - 1 || y == room.height - 1)
                    {
                        grid[y, x] = '#';
                    }
                    else
                    {
                        grid[y, x] = ' ';
                    }
                }
            }

            if (player.startRoomId == room.id) grid[player.startY, player.startX] = 'X';

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

            foreach (var connection in connections)
            {
                if (connection.NORTH == room.id)
                {
                    PlaceDoors(grid, connection.doors, 0, room.width, "horizontal");
                }
                if (connection.SOUTH == room.id)
                {
                    PlaceDoors(grid, connection.doors, room.height - 1, room.width, "horizontal");
                }
                if (connection.WEST == room.id)
                {
                    PlaceDoors(grid, connection.doors, 0, room.height, "vertical");
                }
                if (connection.EAST == room.id)
                {
                    PlaceDoors(grid, connection.doors, room.width - 1, room.height, "vertical");
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
                    else if (grid[y, x] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
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
            Console.WriteLine("\n\n" + new string(' ', leftPadding) + "Lives: " + player.lives + "\n\n\n\n" + new string('-', 50) + "\n" + new string('-', 50));
        }

        static void PlaceDoors(char[,] grid, Door[] doors, int fixedCoord, int variableLimit, string direction)
        {
            foreach (var door in doors)
            {
                int middle = variableLimit / 2;

                if (direction == "horizontal")
                {
                    if (door.type == "colored")
                    {
                        grid[fixedCoord, middle] = '=';
                    }
                    else if (door.type == "open on stones in room")
                    {
                        grid[fixedCoord, middle] = '?';
                    }
                    else if (door.type == "toggle")
                    {
                        grid[fixedCoord, middle] = '⊥';
                    }
                    else if (door.type == "closing gate")
                    {
                        grid[fixedCoord, middle] = '∩';
                    }
                }
                else if (direction == "vertical")
                {
                    if (door.type == "colored")
                    {
                        grid[middle, fixedCoord] = '|';
                    }
                    else if (door.type == "open on stones in room")
                    {
                        grid[middle, fixedCoord] = '?';
                    }
                    else if (door.type == "toggle")
                    {
                        grid[middle, fixedCoord] = '⊥';
                    }
                    else if (door.type == "closing gate")
                    {
                        grid[middle, fixedCoord] = '∩';
                    }
                }
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
            }
            else
            {
                Console.WriteLine("You can't move in that direction.");
            }
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

            return relevantConnection != null;
        }

        static bool MoveToNextRoom(Player player, string direction, Connection[] connections, Room[] rooms)
        {
            var relevantConnections = connections.Where(c =>
                c.NORTH == player.startRoomId || c.SOUTH == player.startRoomId ||
                c.WEST == player.startRoomId || c.EAST == player.startRoomId).ToList();

            foreach (var connection in relevantConnections)
            {
                Room currentRoom = rooms.First(r => r.id == player.startRoomId);
                Room nextRoom;
                switch (direction)
                {
                    case "up" when connection.SOUTH == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.NORTH);
                        player.startRoomId = connection.NORTH;
                        player.startY = nextRoom.height - 1;
                        player.startX = (player.startX - (currentRoom.width / 2)) + (nextRoom.width / 2);
                        break;
                    case "down" when connection.NORTH == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.SOUTH);
                        player.startRoomId = connection.SOUTH;
                        player.startY = 0;
                        player.startX = (player.startX - (currentRoom.width / 2)) + (nextRoom.width / 2);
                        break;
                    case "left" when connection.EAST == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.WEST);
                        player.startRoomId = connection.WEST;
                        player.startX = nextRoom.width - 1;
                        player.startY = (player.startY - (currentRoom.height / 2)) + (nextRoom.height / 2);
                        break;
                    case "right" when connection.WEST == player.startRoomId:
                        nextRoom = rooms.First(r => r.id == connection.EAST);
                        player.startRoomId = connection.EAST;
                        player.startX = 0;
                        player.startY = (player.startY - (currentRoom.height / 2)) + (nextRoom.height / 2);
                        break;
                    default:
                        continue;
                }

                // Ensure player position is within bounds of the new room
                player.startX = Math.Max(0, Math.Min(player.startX, nextRoom.width - 1));
                player.startY = Math.Max(0, Math.Min(player.startY, nextRoom.height - 1));
                return true;
            }

            Console.WriteLine("There's no door in that direction.");
            return false;
        }
    }
}
