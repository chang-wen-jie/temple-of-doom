using TempleOfDoom.Data.DTOs;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ConnectionFactory
{
    public static Connection CreateConnection(ConnectionDto connDto)
    {
        var conn = new Connection();

        var directions = new Dictionary<string, int>
        {
            { CardinalDirection.North, connDto.North },
            { CardinalDirection.South, connDto.South },
            { CardinalDirection.East, connDto.East },
            { CardinalDirection.West, connDto.West },
            { RelativeDirection.Upper, connDto.Upper },
            { RelativeDirection.Lower, connDto.Lower }
        };

        foreach (var (direction, roomId) in directions)
        {
            if (roomId == 0) continue;

            conn.AddRoomDirection(direction, roomId);
        }

        return conn;
    }
}