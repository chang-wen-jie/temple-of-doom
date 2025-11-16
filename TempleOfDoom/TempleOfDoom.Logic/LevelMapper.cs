using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Factories;

namespace TempleOfDoom.Logic;

public class LevelMapper
{
    public Level MapToLevel(RootObject rootObject)
    {
        var level = new Level();

        // Map rooms
        foreach (var roomDto in rootObject.rooms)
        {
            var room = RoomFactory.CreateRoom(roomDto);
            level.Rooms.Add(room);
        }

        // Map connections
        foreach (var conn in rootObject.connections)
        {
            Connection connection = ConnectionFactory.CreateConnection(conn);
            level.AddConnection(connection);
            
            if (conn.NORTH > 0 && conn.SOUTH > 0) 
            {
                var northRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.NORTH);
                var southRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.SOUTH);

                if (northRoom != null && southRoom != null)
                {
                    CreateDoor(northRoom, southRoom.Id, conn.doors, "SOUTH");
                    CreateDoor(southRoom, northRoom.Id, conn.doors, "NORTH");
                }
            }

            // Handle Horizontal (West <-> East)
            if (conn.WEST > 0 && conn.EAST > 0)
            {
                var westRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.WEST);
                var eastRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.EAST);

                if (westRoom != null && eastRoom != null)
                {
                    CreateDoor(westRoom, eastRoom.Id, conn.doors, "EAST");
                    CreateDoor(eastRoom, westRoom.Id, conn.doors, "WEST");
                }
            }
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
    
    private void CreateDoor(Room room, int targetRoomId, DoorDto[] doorDtos, string wallLocation)
    {
        int x = 0, y = 0;

        switch (wallLocation)
        {
            case "NORTH": x = room.Width / 2; y = 0; break;
            case "SOUTH": x = room.Width / 2; y = room.Height - 1; break;
            case "WEST":  x = 0;              y = room.Height / 2; break;
            case "EAST":  x = room.Width - 1; y = room.Height / 2; break;
        }

        // Safe navigation in case 'doors' is null in JSON
        var doorDef = doorDtos?.FirstOrDefault();

        // FIX: Using your 'BaseDoor' class
        var newDoor = new BaseDoor 
        {
            X = x,
            Y = y,
            TargetRoomId = targetRoomId,
            DoorType = doorDef?.type ?? "standard",
            Color = doorDef?.color
        };

        room.Doors.Add(newDoor);
    }
}