using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleOfDoom.Logic;

namespace TempleOfDoom.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = @"..\..\TempleOfDoom.Data\Resources\gameData.json";
            var gameDataService = new GameDataService();

            GameDataDTO gameData = gameDataService.LoadGamedata(jsonFilePath);
            Player newPlayer = gameDataService.CreatePlayer(gameData);
            Console.WriteLine($"Player begint in Room {newPlayer.startRoomId} met coordinaten ({newPlayer.startX}, {newPlayer.startY}) en heeft {newPlayer.lives} levens");
        }
    }
}
