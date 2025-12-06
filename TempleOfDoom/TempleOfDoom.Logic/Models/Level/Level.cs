using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Level;

public class Level
{
    private readonly List<Room> _rooms = [];
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public Player Player { get; private set; } = null!;

    public Room? GetRoom(int id)
    {
        return _rooms.FirstOrDefault(r => r.Id == id);
    }
    
    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
}