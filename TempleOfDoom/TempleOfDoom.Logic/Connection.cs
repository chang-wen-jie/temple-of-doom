namespace TempleOfDoom.Logic;

public class Connection
{
    private Dictionary<Direction, Dictionary<Room, IDoor>> connections;
    
    public Connection()
    {
        connections = new Dictionary<Direction, Dictionary<Room, IDoor>>();
    }

    public void addConnection(Direction direction, Room room, IDoor door)
    {
        if (!connections.ContainsKey(direction))
        {
            connections.Add(direction, new Dictionary<Room, IDoor>());
        }
        
        if (connections[direction].Count == 4)
        {
            throw new InvalidOperationException("Connection already has two rooms");
        }
        
        connections[direction].Add(room, door);
    }
}