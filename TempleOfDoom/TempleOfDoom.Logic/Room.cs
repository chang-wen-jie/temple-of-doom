//namespace TempleOfDoom.Logic;

//public class Room
//{
//    public int Id;
//    public string Type;
//    public int height;
//    public int width;
//    public List<IItem> items;
//    public List<IDoor> doors;

//    public Room(int Id, string Type, int height, int width, List<IItem> items)
//    {
//        this.Id = Id;
//        this.Type = Type;
//        this.height = height;
//        this.width = width;
//        this.items = items;
//    }

//    public void Display(Player player)
//    {
//        int leftPadding = 5;
//        Console.Clear();
//        Console.WriteLine("\n" + new string(' ', leftPadding) + "Welcome to the Temple of Doom!\n" + new string('-', 50) + "\n" + new string('-', 50) + "\n\n\n");

//        // Kamer in een rooster opslaan
//        char[,] grid = new char[height, width];
//        for (int y = 0; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                if (x == 0 || y == 0 || x == width - 1 || y == height - 1) grid[y, x] = '#';
//                else grid[y, x] = ' ';
//            }
//        }

//        foreach (var item in items ?? Enumerable.Empty<IItem>())
//        {
//            if (item != null && item.x >= 0 && item.x < width && item.y >= 0 && item.y < height)
//            {
//                grid[item.y, item.x] = item switch
//                {
//                    SankaraStone => 'S',
//                    Key key => 'K',
//                    PressurePlate => 'T',
//                    BoobyTrap => 'O',
//                    DisappearingBoobyTrap => '@',
//                    _ => '.',
//                };
//            }
//        }

//        if (player.roomId == Id) grid[player.yPosition, player.xPosition] = 'X';

//        // dit vervangen met de nieuwe doors[] methode
//        foreach (var connection in connections)
//        {
//            switch (Id)
//            {
//                case var id when id == connection.SOUTH:
//                    PlaceDoor(grid, connection.doors, 0, width, "horizontal");
//                    break;
//                case var id when id == connection.NORTH:
//                    PlaceDoor(grid, connection.doors, height - 1, width, "horizontal");
//                    break;
//                case var id when id == connection.EAST:
//                    PlaceDoor(grid, connection.doors, 0, height, "vertical");
//                    break;
//                case var id when id == connection.WEST:
//                    PlaceDoor(grid, connection.doors, width - 1, height, "vertical");
//                    break;
//            }
//        }

//        // 2D array uitprinten
//        for (int y = 0; y < height; y++)
//        {
//            Console.Write(new string(' ', leftPadding));
//            for (int x = 0; x < width; x++)
//            {
//                if (grid[y, x] == '#')
//                {
//                    Console.ForegroundColor = ConsoleColor.Yellow;
//                }
//                else if ((y == 0 || y == height - 1 || x == 0 || x == width - 1) && (grid[y, x] == '=' || grid[y, x] == '|'))
//                {
//                    var door = connections.SelectMany(c => c.doors).FirstOrDefault(d => d.type == "colored");
//                    Console.ForegroundColor = door?.color switch
//                    {
//                        "red" => ConsoleColor.Red,
//                        "green" => ConsoleColor.Green,
//                        _ => ConsoleColor.Gray
//                    };
//                }
//                else
//                {
//                    Console.ForegroundColor = ConsoleColor.Gray;
//                }
//                var item = items?.FirstOrDefault(i => i.x == x && i.y == y);

//                if (item != null)
//                {
//                    Console.ForegroundColor = item switch
//                    {
//                        SankaraStone => ConsoleColor.DarkYellow,
//                        Key key => key.Color switch
//                        {
//                            Enums.Color.Red => ConsoleColor.Red,
//                            Enums.Color.Green => ConsoleColor.Green,
//                            _ => ConsoleColor.Gray,
//                        },
//                        _ => ConsoleColor.Gray,
//                    };
//                }
//                Console.Write(grid[y, x] + " ");
//            }
//            Console.WriteLine();
//        }
//        Console.ForegroundColor = ConsoleColor.Gray;
//        Console.WriteLine("\n\n" + new string(' ', leftPadding) + "Lives: " + player.lives + "\n" + new string(' ', leftPadding) + "Stones: " + player.items.Count);
//        Console.WriteLine("\n\n\n" + new string('-', 50) + "\n" + new string('-', 50) + "\n" + new string(' ', leftPadding) + "Inventory");
//        foreach (var key in player.keys)
//        {
//            Console.WriteLine(new string(' ', leftPadding) + key.Color + " key");
//        }
//        Console.WriteLine(new string('-', 50) + "\n" + new string('-', 50));
//    }

//    public void AddDoor(IDoor door)
//    {
//        doors.Add(door);
//    }

//    public void PlaceDoor(char[,] grid, IDoor door, int fixedCoord, int variableLimit, string direction)
//    {
//        int middle = variableLimit / 2;

//        char doorSymbol = doors?.FirstOrDefault()?.Type switch
//        {
//            "colored" => direction == "horizontal" ? '=' : '|',
//            "toggle" => '⊥',
//            "closing gate" => '∩',
//            "open on stones in room" => '?',
//            "open on odd" => '!',
//            _ => ' '
//        };

//        if (direction == "horizontal")
//        {
//            grid[fixedCoord, middle] = doorSymbol;
//        }
//        else if (direction == "vertical")
//        {
//            grid[middle, fixedCoord] = doorSymbol;
//        }
//    }

//    public void RemoveItem(IItem item)
//    {
//        items.Remove(item);
//    }
//}