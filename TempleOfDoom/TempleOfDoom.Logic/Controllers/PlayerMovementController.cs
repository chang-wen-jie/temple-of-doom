using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Helpers;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Controllers;

public class PlayerMovementController(Level level)
{
    public void Move(Player player, Room currentRoom, string direction)
    {
        var (nextX, nextY) = DirectionHelper.GetNextPosition(player.X, player.Y, direction);
        var door = currentRoom.Doors.FirstOrDefault(d => d.X == nextX && d.Y == nextY);

        if (door != null) TryEnterDoor(player, currentRoom, door, direction);
        else MoveInRoom(player, currentRoom, direction, nextX, nextY);
    }

    private static void MoveInRoom(Player player, Room currentRoom, string direction, int targetX, int targetY)
    {
        if (!currentRoom.IsWalkable(targetX, targetY)) return;

        player.SetPosition(targetX, targetY);
        currentRoom.OnPlayerEnter(player);
        HandleIceSliding(player, currentRoom, direction);
    }

    private static void HandleIceSliding(Player player, Room currentRoom, string direction)
    {
        while (currentRoom.HasSpecialTile(player.X, player.Y, SpecialFloorTilesTypes.Ice))
        {
            var (slideX, slideY) = DirectionHelper.GetNextPosition(player.X, player.Y, direction);

            if (!currentRoom.IsWalkable(slideX, slideY)) break;

            player.SetPosition(slideX, slideY);
            currentRoom.OnPlayerEnter(player);
        }
    }

    private void TryEnterDoor(Player player, Room currentRoom, Door door, string direction)
    {
        if (!door.CanEnter(player)) return;

        TransitionToRoom(player, currentRoom, door, direction);
        door.OnEnter();
    }

    private void TransitionToRoom(Player player, Room currentRoom, Door door, string direction)
    {
        var nextRoom = level.GetRoom(door.TargetRoomId);
        int newX, newY;

        if (nextRoom == null) return;

        if (door.TwinDoor != null)
        {
            newX = door.TwinDoor.X;
            newY = door.TwinDoor.Y;
        }
        else
        {
            if (door.DoorType == DoorTypes.Ladder)
                (newX, newY) = CalculateLadderExit(currentRoom, nextRoom);
            else
                (newX, newY) = CalculateDoorExit(player, currentRoom, nextRoom, direction);
        }

        newX = RoomMathHelper.Clamp(newX, nextRoom.Width);
        newY = RoomMathHelper.Clamp(newY, nextRoom.Height);

        player.SetRoom(nextRoom.Id, newX, newY);
    }

    private static (int x, int y) CalculateLadderExit(Room currentRoom, Room nextRoom)
    {
        var destinationLadder = nextRoom.Doors.FirstOrDefault(d =>
            d.DoorType == DoorTypes.Ladder &&
            d.TargetRoomId == currentRoom.Id);

        return destinationLadder != null
            ? (destinationLadder.X, destinationLadder.Y)
            : (nextRoom.Width / 2, nextRoom.Height / 2);
    }

    // Spelerpositie omwisselen om aan tegenovergestelde kant te spawnen
    private static (int x, int y) CalculateDoorExit(Player player, Room currentRoom, Room nextRoom, string direction)
    {
        return direction switch
        {
            Direction.Up => (RoomMathHelper.TranslateX(player.X, currentRoom.Width, nextRoom.Width),
                nextRoom.Height - 1),
            Direction.Down => (RoomMathHelper.TranslateX(player.X, currentRoom.Width, nextRoom.Width), 0),
            Direction.Left => (nextRoom.Width - 1,
                RoomMathHelper.TranslateY(player.Y, currentRoom.Height, nextRoom.Height)),
            Direction.Right => (0, RoomMathHelper.TranslateY(player.Y, currentRoom.Height, nextRoom.Height)),
            _ => (0, 0)
        };
    }
}