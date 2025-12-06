using TempleOfDoom.Data.DTOs;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Factories;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Core;

public static class LevelMapper
{
    public static Level MapToLevel(RootObject rootObject)
    {
        var level = new Level();

        foreach (var roomDto in rootObject.Rooms)
        {
            var room = RoomFactory.CreateRoom(roomDto);

            EnemyFactory.CreateAndAddEnemies(room, roomDto.Enemies);
            level.AddRoom(room);
        }

        foreach (var connDto in rootObject.Connections)
        {
            // Richtingen en kamernummers gekoppeld ophalen
            var connModel = ConnectionFactory.CreateConnection(connDto);

            // DTO meegeven voor objectgegevens
            MapCardinalConnections(level, connDto, connModel);
            MapLadderConnections(level, connDto, connModel);
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

    private static void MapCardinalConnections(Level level, ConnectionDto connDto, Connection connModel)
    {
        // Richting/kamernummer direct meegeven voor minder afhankelijkheid
        if (connModel.HasDirection(CardinalDirection.North) &&
            connModel.HasDirection(CardinalDirection.South))
        {
            var northRoomId = connModel.GetRoomId(CardinalDirection.North);
            var southRoomId = connModel.GetRoomId(CardinalDirection.South);

            CreateTwinDoors(level, northRoomId, southRoomId,
                CardinalDirection.South, CardinalDirection.North, connDto.Doors);
        }

        if (!connModel.HasDirection(CardinalDirection.West) ||
            !connModel.HasDirection(CardinalDirection.East)) return;
        var westRoomId = connModel.GetRoomId(CardinalDirection.West);
        var eastRoomId = connModel.GetRoomId(CardinalDirection.East);

        CreateTwinDoors(level, westRoomId, eastRoomId,
            CardinalDirection.East, CardinalDirection.West, connDto.Doors);
    }

    private static void MapLadderConnections(Level level, ConnectionDto connDto, Connection connectionModel)
    {
        if (!connectionModel.HasDirection(RelativeDirection.Upper) ||
            !connectionModel.HasDirection(RelativeDirection.Lower))
            return;

        var upperRoomId = connectionModel.GetRoomId(RelativeDirection.Upper);
        var lowerRoomId = connectionModel.GetRoomId(RelativeDirection.Lower);

        var upperRoom = level.GetRoom(upperRoomId);
        var lowerRoom = level.GetRoom(lowerRoomId);

        if (upperRoom == null || lowerRoom == null) return;

        var doorUp = CreateLadderDoor(connDto.Ladder.UpperX, connDto.Ladder.UpperY, lowerRoom.Id);
        var doorDown = CreateLadderDoor(connDto.Ladder.LowerX, connDto.Ladder.LowerY, upperRoom.Id);

        // Tweelingladder om uit-/ingangspositie te onthouden
        doorUp.TwinDoor = doorDown;
        doorDown.TwinDoor = doorUp;

        upperRoom.AddDoor(doorUp);
        lowerRoom.AddDoor(doorDown);
    }

    // Tweelingdeur maken om staat van deur over kamers heen te behouden
    private static void CreateTwinDoors(Level level, int room1Id, int room2Id, string dir1, string dir2,
        DoorDto[]? doorDtos)
    {
        var room1 = level.GetRoom(room1Id);
        var room2 = level.GetRoom(room2Id);

        if (room1 == null || room2 == null) return;

        var door1 = CreateBaseDoor(room1, room2.Id, dir1, doorDtos);
        var door2 = CreateBaseDoor(room2, room1.Id, dir2, doorDtos);

        door1.TwinDoor = door2;
        door2.TwinDoor = door1;

        room1.AddDoor(DoorFactory.CreateDecoratedDoor(door1, doorDtos, room1));
        room2.AddDoor(DoorFactory.CreateDecoratedDoor(door2, doorDtos, room2));
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
        foreach (var room in level.Rooms) room.ConnectMechanisms();
    }
}