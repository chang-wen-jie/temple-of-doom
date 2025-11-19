namespace TempleOfDoom.Logic;

using Models;
using Models.Doors;

public class GameManager(Level level)
{
    private readonly Level _level = level;

    public void HandlePlayerInput(string inputDirection)
    {
        var player = _level.Player;
        var currentRoom = _level.Rooms.First(r => r.Id == player.StartRoomId);
        var (nextX, nextY) = CalculateNextPosition(player, inputDirection);
        var door = currentRoom.Doors.FirstOrDefault(d => d.X == nextX && d.Y == nextY);

        if (door != null)
        {
            TryEnterDoor(player, currentRoom, door, inputDirection);
        }
        else
        {
            player.Move(currentRoom, inputDirection);
        }
    }

    private void TryEnterDoor(Player player, Room currentRoom, Door door, string direction)
    {
        // FIX 1: Remove 'if (door is BaseDoor)'.
        // We trust the polymorphic 'CanEnter' to handle Keys, Lives, etc.
        if (!door.CanEnter(player))
        {
            return; 
        }

        // FIX 2: Move the player BEFORE triggering post-entry effects
        ChangeRoom(player, currentRoom, door, direction);
        
        // FIX 3: Trigger the side effects (like Closing Gate)
        // Ensure your abstract Door class has the virtual 'OnEnter' method we discussed.
        door.OnEnter();
    }

    private void ChangeRoom(Player player, Room currentRoom, Door door, string direction)
    {
        var nextRoom = _level.Rooms.First(r => r.Id == door.TargetRoomId);

        var newX = 0;
        var newY = 0;

        switch (direction)
        {
            case "up":
                newX = CalculateCenteredX(player.StartXPos, currentRoom.Width, nextRoom.Width);
                newY = nextRoom.Height - 1; 
                break;
            case "down":
                newX = CalculateCenteredX(player.StartXPos, currentRoom.Width, nextRoom.Width);
                newY = 0;
                break;
            case "left":
                newX = nextRoom.Width - 1;
                newY = CalculateCenteredY(player.StartYPos, currentRoom.Height, nextRoom.Height);
                break;
            case "right":
                newX = 0;
                newY = CalculateCenteredY(player.StartYPos, currentRoom.Height, nextRoom.Height);
                break;
        }

        newX = Math.Max(0, Math.Min(newX, nextRoom.Width - 1));
        newY = Math.Max(0, Math.Min(newY, nextRoom.Height - 1));
        player.SetRoom(nextRoom.Id, newX, newY);
    }

    private static int CalculateCenteredX(int currentX, int currentW, int nextW)
    {
        return (currentX - (currentW / 2)) + (nextW / 2);
    }

    private static int CalculateCenteredY(int currentY, int currentH, int nextH)
    {
        return (currentY - (currentH / 2)) + (nextH / 2);
    }

    private static (int, int) CalculateNextPosition(Player p, string dir)
    {
        return dir switch
        {
            "up" => (p.StartXPos, p.StartYPos - 1),
            "down" => (p.StartXPos, p.StartYPos + 1),
            "left" => (p.StartXPos - 1, p.StartYPos),
            "right" => (p.StartXPos + 1, p.StartYPos),
            _ => (p.StartXPos, p.StartYPos)
        };
    }
}