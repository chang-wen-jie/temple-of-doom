using TempleOfDoom.Data;
using TempleOfDoom.Logic;
using TempleOfDoom.Logic.Models;

namespace TempleOfDoom.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        var levelLoader = new LevelLoader();
        var levelMapper = new LevelMapper();
        
        RootObject rootObject = levelLoader.LoadLevel("GameData.json");
        Level level = levelMapper.MapToLevel(rootObject);
        Console.WriteLine("Done");
    }
}