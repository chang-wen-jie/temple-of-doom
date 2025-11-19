using TempleOfDoom.Data;
using TempleOfDoom.Logic.Events;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Factories;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic;

public class LevelLoader
{
    public Level MapToLevel(RootObject rootObject)
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
            
            if (conn.NORTH > 0 && conn.SOUTH > 0) 
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

            // Handle Horizontal (West <-> East)
            if (conn.WEST > 0 && conn.EAST > 0)
            {
                var westRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.WEST);
                var eastRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.EAST);

                if (westRoom != null && eastRoom != null)
                {
                    // A. Create the raw BaseDoors
                    var doorWest = CreateBaseDoor(westRoom, eastRoom.Id, "EAST", conn.doors);
                    var doorEast = CreateBaseDoor(eastRoom, westRoom.Id, "WEST", conn.doors);

                    // B. Link them
                    doorWest.TwinDoor = doorEast;
                    doorEast.TwinDoor = doorWest;

                    // C. Decorate and Add
                    westRoom.Doors.Add(ApplyDecorators(doorWest, conn.doors));
                    eastRoom.Doors.Add(ApplyDecorators(doorEast, conn.doors));
                }
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
    
    private BaseDoor CreateBaseDoor(Room room, int targetRoomId, string wallLocation, DoorDto[] doorDtos)
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
    
    private Door ApplyDecorators(Door door, DoorDto[] doorDtos)
    {
        if (doorDtos == null) return door;

        var finalDoor = door;

        foreach (var dto in doorDtos)
        {
            // Use your Factory to wrap the door
            finalDoor = DoorFactory.DecorateDoor(finalDoor, dto);
        }

        return finalDoor;
    }
    
    private void ConnectMechanisms(Level level)
    {
        foreach (var room in level.Rooms)
        {
            // 1. Find all Pressure Plates in this room
            // We use OfType to filter the generic IItem list
            var plates = room.Items.OfType<PressurePlate>().ToList();

            // If there are no plates, move to the next room
            if (!plates.Any()) continue;

            // 2. Find all Doors in this room that are Observers (Toggle Doors)
            // Note: This looks for doors where the OUTERMOST layer implements IObserver
            var toggleDoors = room.Doors.OfType<IObserver>().ToList();

            // 3. Wire them together
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