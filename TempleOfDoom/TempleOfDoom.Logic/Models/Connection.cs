using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models;

public class Connection
{
    private readonly Dictionary<string, int> _roomDirections = new();
    
    public void AddRoomDirection(string direction, int roomId)
    {
        _roomDirections[direction.ToLower()] = roomId;
    }
    
    public IEnumerable<KeyValuePair<string, int>> GetRoomDirections()
    {
        return _roomDirections;
    }
}