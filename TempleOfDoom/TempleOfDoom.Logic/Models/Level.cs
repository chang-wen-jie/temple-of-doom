namespace TempleOfDoom.Logic.Models;

public class Level
{
    public List<Room> Rooms { get; } = new();
    public List<Connection> Connections { get; } = new();
    public Player Player { get; private set; }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
    
    public void AddConnection(Connection connection)
    {
        Connections.Add(connection);
    }
}