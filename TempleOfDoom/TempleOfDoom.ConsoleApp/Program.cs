
using TempleOfDoom.Data;
using TempleOfDoom.Logic;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.UI.Rendering;

namespace TempleOfDoom.ConsoleApp
{
    internal abstract class Program
    {
        private static void Main()
        {
            try 
            {
                RunGame();
            }
            catch (Exception ex)
            {
                HandleGameError(ex);
            }
        }

        private static void RunGame()
        {
            var levelLoader = new LevelLoader();
            var levelMapper = new LevelMapper();
            var roomRenderer = new RoomRenderer();
            
            var rootObject = levelLoader.LoadLevel("GameData.json");
            var level = levelMapper.MapToLevel(rootObject);
            
            var startRoom = level.Rooms.First(r => r.Id == level.Player.StartRoomId);
            var player = level.Player;
            
            var gameRunning = true;
            var collectedSankaraStones = 0;

            while (gameRunning && player.Lives > 0 && collectedSankaraStones < 5)
            {
                roomRenderer.DisplayRoom(startRoom, player);

                var key = Console.ReadKey(true).Key;
                var direction = MapKeyToDirection(key);

                if (string.IsNullOrEmpty(direction)) continue;
                
                player.MovePlayer(startRoom, direction);

                collectedSankaraStones = player.GetItems()
                    .OfType<SankaraStone>()
                    .Count();
            }
            
            DisplayGameResult(collectedSankaraStones);
        }

        private static string MapKeyToDirection(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow => "up",
                ConsoleKey.DownArrow => "down",
                ConsoleKey.LeftArrow => "left",
                ConsoleKey.RightArrow => "right",
                _ => ""
            };
        }

        private static void DisplayGameResult(int collectedSankaraStones)
        {
            Console.Clear();
            var resultMessage = collectedSankaraStones == 5 
                ? "YOU WON" 
                : "GAME OVER";

            Console.WriteLine(
                new string('*', 50) + "\n" + 
                new string(' ', 5) + resultMessage + "\n" + 
                new string('*', 50)
            );
        }

        private static void HandleGameError(Exception ex)
        {
            Console.WriteLine("An error occurred during the game:");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Please check the game configuration and try again.");
        }
    }
}