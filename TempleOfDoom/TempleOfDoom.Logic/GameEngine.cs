using System.Text.Json;
using TempleOfDoom.Data;
using TempleOfDoom.Logic.Services;

namespace TempleOfDoom.Logic;

public class GameEngine
{
    public void run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "gameData.json");
        _ = new GameDataService();
            
        string jsonData = File.ReadAllText(jsonFilePath);
            
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var rootDto = JsonSerializer.Deserialize<GameDataDTO>(jsonData, options);
        
        List<Room> rooms = rootDto.rooms.Select(dto => new Room(
            dto.id,
            dto.type,
            dto.width,
            dto.height,
            dto.items.Select<ItemDto, tem>(itemDto => new IItem(
                itemDto.Type,
                itemDto.Damage,
                itemDto.Color,
                itemDto.X,
                itemDto.Y
            )).ToList()
        )).ToList();
    }
}