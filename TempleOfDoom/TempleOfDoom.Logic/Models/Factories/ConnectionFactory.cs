using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ConnectionFactory
{
    public static Connection CreateConnection(ConnectionDto connectionDto)
    {
        var connection = new Connection();
        var doors = connectionDto.doors;

        // Define all possible directions
        var directions = new Dictionary<string, int>
        {
            { "north", connectionDto.NORTH },
            { "south", connectionDto.SOUTH },
            { "east", connectionDto.EAST },
            { "west", connectionDto.WEST }
        };
        
        var doorIndex = 0;
        foreach (var (direction, roomId) in directions)
        {
            if (roomId == 0)
            {
                continue;
            }
            
            // Create door if available and there are doors left to assign
            Door? door = null;
            if (doorIndex < doors.Length)
            {
                door = DoorFactory.CreateDoor(doors[doorIndex++]);
            }

            connection.AddRoomDirection(direction, roomId, door);
        }

        return connection;
    }
}