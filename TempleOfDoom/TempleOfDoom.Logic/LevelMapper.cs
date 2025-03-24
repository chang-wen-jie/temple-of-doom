using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Factories;

namespace TempleOfDoom.Logic;

public class LevelMapper
{
    public Level MapToLevel(RootObject rootObject)
    {
        var level = new Level();

        // Map rooms
        foreach (RoomDto roomDto in rootObject.rooms)
        {
            Room room = RoomFactory.CreateRoom(roomDto);
            level.Rooms.Add(room);
        }

        // Map connections
        foreach (ConnectionDto connectionDto in rootObject.connections)
        {
            Connection connection = ConnectionFactory.CreateConnection(connectionDto);
            level.AddConnection(connection);
        }
        
        // Map player
        var player = new Player(
            rootObject.player.startRoomId,
            rootObject.player.startX,
            rootObject.player.startY,
            rootObject.player.lives
        );
        
        level.setPlayer(player);
        
        return level;
    }
}