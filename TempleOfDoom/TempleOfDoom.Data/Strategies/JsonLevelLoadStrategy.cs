using System.Text.Json;
using TempleOfDoom.Data.DTOs;

namespace TempleOfDoom.Data.Strategies;

public class JsonLevelLoadStrategy : ILevelLoadStrategy
{
    public RootObject LoadLevel(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("Bestand niet gevonden", filePath);

        var json = File.ReadAllText(filePath);
        
        return JsonSerializer.Deserialize<RootObject>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        }) ?? throw new Exception("Deserialiseren van JSON gefaald");
    }
}