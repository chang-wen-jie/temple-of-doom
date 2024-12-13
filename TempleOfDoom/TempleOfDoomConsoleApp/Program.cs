using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoomConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Pad dynamisch maken
            string jsonFilePath = @"C:\Users\Chang Wen Jie\source\repos\deelopdracht-1-24-25-temple-of-doom-mohammedbogatyrev_wenjiechang\TempleOfDoom\TempleOfDoom.Data\Resources\gameData.json";
            _ = new GameDataService();

            GameDataDTO gameData = GameDataService.LoadGameData(jsonFilePath);
            Player newPlayer = GameDataService.CreatePlayer(gameData);
            Room newRoom = GameDataService.CreateRoom(gameData, 1);

            Console.WriteLine($"Room: {newPlayer.startRoomId}, Player Coordinates: ({newPlayer.startX}, {newPlayer.startY}), Lives: {newPlayer.lives}");
            Console.WriteLine($"Room {newRoom.id} ({newRoom.type}) is {newRoom.width}x{newRoom.height} and contains {newRoom.items.Length} items.");
        }
    }
}
