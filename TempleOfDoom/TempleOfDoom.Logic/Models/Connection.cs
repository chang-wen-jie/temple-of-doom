using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models;

public class Connection
{
    private Dictionary<string, (int RoomdId, Door? Door)> _roomDirections = new();
    
    public void AddRoomDirection(string direction, int roomId, Door? door)
    {
        _roomDirections[direction.ToLower()] = (roomId, door);
    }
    
    public IEnumerable<KeyValuePair<string, (int RoomId, Door? Door)>> GetRoomDirections()
    {
        return _roomDirections;
    }
}