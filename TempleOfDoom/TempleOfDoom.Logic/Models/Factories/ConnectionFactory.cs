using TempleOfDoom.Data;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ConnectionFactory
{
    public static Connection CreateConnection(ConnectionDto connectionDto)
    {
        var connection = new Connection();

        var directions = new Dictionary<string, int>
        {
            { CardinalDirection.North, connectionDto.North },
            { CardinalDirection.South, connectionDto.South },
            { CardinalDirection.East, connectionDto.EAST },
            { CardinalDirection.West, connectionDto.West }
        };

        foreach (var (direction, roomId) in directions)
        {
            if (roomId == 0) continue;

            connection.AddRoomDirection(direction, roomId);
        }

        return connection;
    }
}