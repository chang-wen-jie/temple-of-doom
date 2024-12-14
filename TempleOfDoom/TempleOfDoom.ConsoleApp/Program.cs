using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoom.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Deursymbolen uitprintbaar maken
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Pad dynamisch maken
            string jsonFilePath = @"C:\Users\Chang Wen Jie\source\repos\deelopdracht-1-24-25-temple-of-doom-mohammedbogatyrev_wenjiechang\TempleOfDoom\TempleOfDoom.Data\Resources\gameData.json";
            _ = new GameDataService();

            try
            {
                GameDataDTO gameData = GameDataService.LoadGameData(jsonFilePath);

                Player newPlayer = GameDataService.CreatePlayer(gameData.player);
                DisplayPlayerInfo(newPlayer);

                foreach (var room in gameData.rooms)
                {
                    Room newRoom = GameDataService.CreateRoom(room);
                    DisplayRoom(newRoom, gameData.connections);
                }

                foreach (var connection in gameData.connections)
                {
                    Connection newConnection = GameDataService.CreateConnection(connection);
                    DisplayConnection(newConnection);
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GameDataDTO foutmelding: {ex.Message}");
            }
        }

        static void DisplayPlayerInfo(Player player)
        {
            Console.WriteLine($"Start Room ID: {player.startRoomId}");
            Console.WriteLine($"Coordinates: X{player.startX} Y{player.startY}");
            Console.WriteLine($"Lives: {player.lives}");
            Console.WriteLine(new string('*', 40));
        }

        static void DisplayRoom(Room room, Connection[] connections)
        {
            Console.WriteLine($"Room ID: {room.id}");

            // Room in 2D array genereren
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

            foreach (var item in room.items)
            {
                if (item.x >= 0 && item.x < room.width && item.y >= 0 && item.y < room.height)
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

            // DEUREN: open on odd & open on stones in room (?), colored: verticaal (| + kleur)/horizontaal (= + kleur), toggle(⊥), closing gate(∩)
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
                for (int x = 0; x < room.width; x++)
                {
                    Console.Write(grid[y, x] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(new string('-', 40));
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

        static void DisplayConnection(Connection connection)
        {
            Console.WriteLine("Connection:");
            Console.WriteLine($"North: {connection.NORTH}");
            Console.WriteLine($"South: {connection.SOUTH}");
            Console.WriteLine($"East: {connection.EAST}");
            Console.WriteLine($"West: {connection.WEST}");
            Console.WriteLine("Doors:");

            foreach (var door in connection.doors)
            {
                Console.WriteLine($"\tType: {door.type}, " + (!string.IsNullOrEmpty(door.color) ? $"Color: {door.color}," : "" + $"No of Stones: {door.no_of_stones}"));
            }
            Console.WriteLine(new string('-', 40));
        }
    }
}
