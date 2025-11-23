using TempleOfDoom.Data;
using TempleOfDoom.Logic.Events;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Factories;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic;

public abstract class LevelMapper
{
    public static Level MapToLevel(RootObject rootObject)
    {
        var level = new Level();

        foreach (var roomDto in rootObject.rooms)
        {
            var room = RoomFactory.CreateRoom(roomDto);
            level.Rooms.Add(room);
        }

        foreach (var conn in rootObject.connections)
        {
            var connection = ConnectionFactory.CreateConnection(conn);
            level.AddConnection(connection);
            
            if (conn is { NORTH: > 0, SOUTH: > 0 }) 
            {
                var northRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.NORTH);
                var southRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.SOUTH);

                if (northRoom != null && southRoom != null)
                {
                    // A. Create the raw BaseDoors
                    var doorNorth = CreateBaseDoor(northRoom, southRoom.Id, "SOUTH", conn.doors);
                    var doorSouth = CreateBaseDoor(southRoom, northRoom.Id, "NORTH", conn.doors);

                    // B. Link them (The Twin Logic)
                    doorNorth.TwinDoor = doorSouth;
                    doorSouth.TwinDoor = doorNorth;

                    // C. Decorate and Add (The Logic Wrappers)
                    northRoom.Doors.Add(ApplyDecorators(doorNorth, conn.doors));
                    southRoom.Doors.Add(ApplyDecorators(doorSouth, conn.doors));
                }
            }

            if (conn is not { WEST: > 0, EAST: > 0 }) continue;
            {
                var westRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.WEST);
                var eastRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.EAST);

                if (westRoom == null || eastRoom == null) continue;
                var doorWest = CreateBaseDoor(westRoom, eastRoom.Id, "EAST", conn.doors);
                var doorEast = CreateBaseDoor(eastRoom, westRoom.Id, "WEST", conn.doors);

                doorWest.TwinDoor = doorEast;
                doorEast.TwinDoor = doorWest;

                westRoom.Doors.Add(ApplyDecorators(doorWest, conn.doors));
                eastRoom.Doors.Add(ApplyDecorators(doorEast, conn.doors));
            }
        }
        
        var player = new Player(
            rootObject.player.startRoomId,
            rootObject.player.startX,
            rootObject.player.startY,
            rootObject.player.lives
        );
        
        level.setPlayer(player);
        
        ConnectMechanisms(level);
        
        return level;
    }
    
    private static BaseDoor CreateBaseDoor(Room room, int targetRoomId, string wallLocation, DoorDto[] doorDtos)
    {
        int x = 0, y = 0;

        switch (wallLocation)
        {
            case "NORTH": x = room.Width / 2; y = 0; break;
            case "SOUTH": x = room.Width / 2; y = room.Height - 1; break;
            case "WEST":  x = 0;              y = room.Height / 2; break;
            case "EAST":  x = room.Width - 1; y = room.Height / 2; break;
        }

        var doorDef = doorDtos?.FirstOrDefault();

        return new BaseDoor 
        {
            X = x,
            Y = y,
            TargetRoomId = targetRoomId,
            // We set these strings so the Renderer knows what symbol/color to draw
            // even if the logic is handled by decorators later.
            DoorType = doorDef?.type ?? "standard",
            Color = doorDef?.color
        };
    }
    
    private static Door ApplyDecorators(Door door, DoorDto[] doorDtos)
    {
        if (doorDtos == null) return door;

        var finalDoor = door;

        foreach (var dto in doorDtos)
        {
            finalDoor = DoorFactory.DecorateDoor(finalDoor, dto);
        }

        return finalDoor;
    }
    
    private static void ConnectMechanisms(Level level)
    {
        foreach (var room in level.Rooms)
        {
            var plates = room.Items.OfType<PressurePlate>().ToList();

            if (plates.Count == 0) continue;

            var toggleDoors = room.Doors.OfType<IObserver>().ToList();

            foreach (var plate in plates)
            {
                foreach (IObserver doorObserver in toggleDoors)
                {
                    plate.Attach(doorObserver);
                }
            }
        }
    }
}