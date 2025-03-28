using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.UI.Rendering
{
    public class RoomRenderer
    {
        private const int LeftPadding = 5;

        public void DisplayRoom(Room room, Player player)
        {
            Console.Clear();
            PrintHeader();
            
            var grid = CreateRoomGrid(room);
            PopulateGridWithItems(room, grid);
            PlacePlayerOnGrid(player, grid);
            
            PrintGrid(grid);
            DisplayPlayerStats(player);
        }

        private static void PrintHeader()
        {
            Console.WriteLine(
                "\n" + 
                new string(' ', LeftPadding) + "Welcome to the Temple of Doom!\n" + 
                new string('-', 50) + "\n" + 
                new string('-', 50) + "\n\n\n"
            );
        }

        private char[,] CreateRoomGrid(Room room)
        {
            var grid = new char[room.Height, room.Width];
            
            for (var y = 0; y < room.Height; y++)
            {
                for (var x = 0; x < room.Width; x++)
                {
                    grid[y, x] = IsBorder(x, y, room.Width, room.Height) 
                        ? '#' 
                        : ' ';
                }
            }
            
            return grid;
        }

        private static bool IsBorder(int x, int y, int width, int height)
        {
            return x == 0 || y == 0 || x == width - 1 || y == height - 1;
        }

        private void PopulateGridWithItems(Room room, char[,] grid)
        {
            foreach (var item in room.Items.Where(item => IsValidPosition(item, room.Width, room.Height)))
            {
                grid[item.YPos, item.XPos] = GetItemSymbol(item);
            }
        }

        private bool IsValidPosition(IItem item, int width, int height)
        {
            return item.XPos >= 0 && item.XPos < width && 
                   item.YPos >= 0 && item.YPos < height;
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

        private static void PlacePlayerOnGrid(Player player, char[,] grid)
        {
            grid[player.StartYPos, player.StartXPos] = 'X';
        }

        private void PrintGrid(char[,] grid)
        {
            for (var y = 0; y < grid.GetLength(0); y++)
            {
                Console.Write(new string(' ', LeftPadding));
                for (var x = 0; x < grid.GetLength(1); x++)
                {
                    SetConsoleColor(grid[y, x]);
                    Console.Write(grid[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void SetConsoleColor(char gridItem)
        {
            Console.ForegroundColor = gridItem switch
            {
                '#' => ConsoleColor.Yellow,
                'S' => ConsoleColor.DarkYellow,
                'K' => ConsoleColor.Green,
                'T' => ConsoleColor.Red,
                'O' => ConsoleColor.DarkRed,
                '@' => ConsoleColor.Magenta,
                _ => ConsoleColor.Gray
            };
        }

        private static void DisplayPlayerStats(Player player)
        {
            Console.WriteLine($"\n\n{new string(' ', LeftPadding)}Lives: {player.Lives}");
            
            Console.WriteLine($"\n\n\n{new string('-', 50)}\n{new string('-', 50)}\n{new string(' ', LeftPadding)}Inventory");
            
            var keys = player.GetItems().OfType<Key>();
            foreach (var key in keys)
            {
                Console.WriteLine($"{new string(' ', LeftPadding)}{key.Color} key");
            }
            
            Console.WriteLine($"{new string('-', 50)}\n{new string('-', 50)}");
        }
    }
}