namespace TempleOfDoom.Logic.Models.Level;

public class Connection
{
    private readonly Dictionary<string, int> _roomDirections = new();

    // Bestaande kamernummer en richting koppelen
    public void AddRoomDirection(string direction, int roomId)
    {
        _roomDirections[direction.ToLower()] = roomId;
    }

    public int GetRoomId(string direction)
    {
        return _roomDirections.GetValueOrDefault(direction.ToLower(), -1);
    }

    public bool HasDirection(string direction)
    {
        return _roomDirections.ContainsKey(direction.ToLower());
    }
}