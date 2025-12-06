using TempleOfDoom.Data.Factories;
using TempleOfDoom.Data.Loaders;
using TempleOfDoom.Logic.Core;
using TempleOfDoom.UI.Inputs;
using TempleOfDoom.UI.Rendering;

namespace TempleOfDoom.ConsoleApp;

internal static class Program
{
    // Kopie van bestand want program laadt alleen vanuit eigen map
    private const string LevelFileName = "GameData.json";

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
        // Bestand op basis van extensie doorwijzen
        var strategy = LevelStrategyFactory.GetStrategy(LevelFileName);
        var dataLoader = new LevelLoader(strategy);
        var levelDto = dataLoader.LoadLevel(LevelFileName);
        var level = LevelMapper.MapToLevel(levelDto);
        var gameManager = new GameManager(level);
        
        while (!gameManager.IsGameOver)
        {
            var currentRoom = gameManager.GetCurrentRoom();

            RoomRenderer.RenderRoom(currentRoom, level.Player);

            var direction = InputReader.GetDirection();

            if (string.IsNullOrEmpty(direction)) continue;

            gameManager.HandlePlayerInput(direction);
        }

        var resultMessage = gameManager.HasWon ? "GEWONNEN!" : "VERLOREN!";
        RoomRenderer.RenderMessage(resultMessage);
    }
}