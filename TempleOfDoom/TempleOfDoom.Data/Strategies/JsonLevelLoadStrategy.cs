using System.Text.Json;

namespace TempleOfDoom.Data.Strategies;

public class JsonLevelLoadStrategy : ILevelLoadStrategy
{
    public RootObject LoadLevel(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

        var json = File.ReadAllText(filePath);
        
        return JsonSerializer.Deserialize<RootObject>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        }) ?? throw new Exception("Failed to deserialize JSON");
    }
}