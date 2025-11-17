
using TempleOfDoom.Data;
using TempleOfDoom.Logic;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.UI.Rendering;
using LevelLoader = TempleOfDoom.Logic.LevelLoader;

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
            var levelLoader = new Data.LevelLoader();
            var levelMapper = new LevelLoader();
            
            var rootObject = levelLoader.LoadLevel("GameData.json");
            var level = levelMapper.MapToLevel(rootObject);
            
            var gameManager = new GameManager(level);
            var player = level.Player;
            
            var collectedSankaraStones = 0;

            while (player.Lives > 0 && collectedSankaraStones < 5)
            {
                var currentRoom = level.Rooms.First(r => r.Id == player.StartRoomId);
                
                RoomRenderer.RenderRoom(currentRoom, player);

                var key = Console.ReadKey(true).Key;
                var direction = MapKeyToDirection(key);

                if (string.IsNullOrEmpty(direction)) continue;
                
                gameManager.HandlePlayerInput(direction);

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
            var resultMessage = collectedSankaraStones == 5 
                ? "GAME CLEARED" 
                : "GAME OVER";

            Console.WriteLine(
                "\n" + new string('*', 50) + "\n" + 
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