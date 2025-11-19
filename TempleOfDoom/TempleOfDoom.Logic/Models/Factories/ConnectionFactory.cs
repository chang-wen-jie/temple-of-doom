using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ConnectionFactory
{
    public static Connection CreateConnection(ConnectionDto connectionDto)
    {
        var connection = new Connection();

        var directions = new Dictionary<string, int>
        {
            { "north", connectionDto.NORTH },
            { "south", connectionDto.SOUTH },
            { "east", connectionDto.EAST },
            { "west", connectionDto.WEST }
        };
        
        foreach (var (direction, roomId) in directions)
        {
            if (roomId == 0)
            {
                continue;
            }
            
            connection.AddRoomDirection(direction, roomId);
        }

        return connection;
    }
}