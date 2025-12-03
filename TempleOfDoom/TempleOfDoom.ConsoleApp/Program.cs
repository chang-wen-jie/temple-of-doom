using TempleOfDoom.Data;
using TempleOfDoom.Data.Strategies;
using TempleOfDoom.Logic;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.UI.Inputs;
using TempleOfDoom.UI.Rendering;

namespace TempleOfDoom.ConsoleApp
{
    internal static class Program
    {
        private static void Main()
        {
            try 
            {
                RunGame();
            }
            catch (Exception ex)
            {
                RoomRenderer.RenderMessage("Een fout heeft zich opgetreden: " + ex.Message);
            }
        }

        private static void RunGame()
        {
            const string fileName = "GameData.json";
            
            ILevelLoadStrategy strategy = fileName.EndsWith(".json")
                ? new JsonLevelLoadStrategy()
                : new XmlLevelLoadingStrategy();
            
            var dataLoader = new LevelLoader(strategy);
            var rootObject = dataLoader.LoadLevel(fileName);
            var level = LevelMapper.MapToLevel(rootObject);
            var gameManager = new GameManager(level);
            var player = level.Player;
            var collectedSankaraStones = 0;

            while (player.Lives > 0 && collectedSankaraStones < 5)
            {
                var currentRoom = level.Rooms.First(r => r.Id == player.CurrentRoomId);
                
                RoomRenderer.RenderRoom(currentRoom, player);

                var direction = InputReader.GetDirection();

                if (string.IsNullOrEmpty(direction)) continue;
                
                gameManager.HandlePlayerInput(direction);

                collectedSankaraStones = player.Inventory
                    .OfType<SankaraStone>()
                    .Count();
            }
            
            var resultMessage = collectedSankaraStones == 5 ? "GEWONNEN!" : "VERLOREN!";
            RoomRenderer.RenderMessage(resultMessage);
        }
    }
}