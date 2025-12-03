using TempleOfDoom.Data;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Events;
using TempleOfDoom.Logic.Models;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Factories;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic;

public static class LevelMapper
{
    public static Level MapToLevel(RootObject rootObject)
    {
        var level = new Level();

        foreach (var roomDto in rootObject.Rooms)
        {
            var room = RoomFactory.CreateRoom(roomDto);

            EnemyFactory.CreateAndAddEnemies(room, roomDto.Enemies);
            level.Rooms.Add(room);
        }

        foreach (var conn in rootObject.Connections)
        {
            level.AddConnection(ConnectionFactory.CreateConnection(conn));
            MapCardinalConnections(level, conn);
            MapLadderConnections(level, conn);
        }

        var player = new Player(
            rootObject.Player.StartRoomId,
            rootObject.Player.StartX,
            rootObject.Player.StartY,
            rootObject.Player.Lives
        );

        level.SetPlayer(player);
        ConnectMechanisms(level);

        return level;
    }

    private static void MapCardinalConnections(Level level, ConnectionDto conn)
    {
        if (conn is { North: > 0, South: > 0 })
            CreateTwinDoors(level, conn.North, conn.South,
                CardinalDirection.South, CardinalDirection.North, conn.Doors);

        if (conn is { West: > 0, EAST: > 0 })
            CreateTwinDoors(level, conn.West, conn.EAST,
                CardinalDirection.East, CardinalDirection.West, conn.Doors);
    }

    private static void MapLadderConnections(Level level, ConnectionDto conn)
    {
        if (conn.Upper <= 0 || conn.Lower <= 0) return;

        var upperRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.Upper);
        var lowerRoom = level.Rooms.FirstOrDefault(r => r.Id == conn.Lower);

        if (upperRoom == null || lowerRoom == null) return;

        var doorUp = CreateLadderDoor(conn.Ladder.UpperX, conn.Ladder.UpperY, lowerRoom.Id);
        var doorDown = CreateLadderDoor(conn.Ladder.LowerX, conn.Ladder.LowerY, upperRoom.Id);

        doorUp.TwinDoor = doorDown;
        doorDown.TwinDoor = doorUp;

        upperRoom.AddDoor(doorUp);
        lowerRoom.AddDoor(doorDown);
    }

    // Staat van Toggle deuren onthouden
    private static void CreateTwinDoors(Level level, int room1Id, int room2Id, string dir1, string dir2,
        DoorDto[]? doorDtos)
    {
        var room1 = level.Rooms.FirstOrDefault(r => r.Id == room1Id);
        var room2 = level.Rooms.FirstOrDefault(r => r.Id == room2Id);

        if (room1 == null || room2 == null) return;

        var door1 = CreateBaseDoor(room1, room2.Id, dir1, doorDtos);
        var door2 = CreateBaseDoor(room2, room1.Id, dir2, doorDtos);

        door1.TwinDoor = door2;
        door2.TwinDoor = door1;

        room1.AddDoor(DoorFactory.CreateDecoratedDoor(door1, doorDtos));
        room2.AddDoor(DoorFactory.CreateDecoratedDoor(door2, doorDtos));
    }

    private static BaseDoor CreateBaseDoor(Room room, int targetRoomId, string wallLocation, DoorDto[]? doorDtos)
    {
        int x = 0, y = 0;

        switch (wallLocation)
        {
            case CardinalDirection.North:
                x = room.Width / 2;
                y = 0;
                break;
            case CardinalDirection.South:
                x = room.Width / 2;
                y = room.Height - 1;
                break;
            case CardinalDirection.West:
                x = 0;
                y = room.Height / 2;
                break;
            case CardinalDirection.East:
                x = room.Width - 1;
                y = room.Height / 2;
                break;
        }

        var doorDef = doorDtos?.FirstOrDefault();

        return new BaseDoor
        {
            X = x,
            Y = y,
            TargetRoomId = targetRoomId,
            DoorType = doorDef?.Type ?? null,
            Color = doorDef?.Color
        };
    }

    private static BaseDoor CreateLadderDoor(int x, int y, int targetRoomId)
    {
        return new BaseDoor
        {
            X = x,
            Y = y,
            TargetRoomId = targetRoomId,
            DoorType = DoorTypes.Ladder
        };
    }

    private static void ConnectMechanisms(Level level)
    {
        foreach (var room in level.Rooms)
        {
            var pressurePlates = room.Items.OfType<PressurePlate>().ToList();

            if (pressurePlates.Count == 0) continue;

            var toggleDoors = room.Doors.OfType<IObserver>().ToList();

            foreach (var plate in pressurePlates)
            foreach (var doorObserver in toggleDoors)
                plate.Attach(doorObserver);
        }
    }
}