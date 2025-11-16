using System.Text.Json;

namespace TempleOfDoom.Data.Strategies;

public class JsonLevelLoadStrategy : ILevelLoadStrategy
{
    public RootObject LoadLevel(string filePath)
    {
        var json = File.ReadAllText(filePath);
        try
        {
            return JsonSerializer.Deserialize<RootObject>(json) ?? throw new Exception("Kan GameData.json niet inlezen");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}