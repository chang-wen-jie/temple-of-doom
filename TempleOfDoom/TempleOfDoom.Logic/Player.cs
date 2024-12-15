using System.Numerics;
using TempleOfDoom.Data;
using TempleOfDoom.Logic.Enums;

namespace TempleOfDoom.Logic;

public class Player
{
    // keys en items op public gezet voor nu
    public int lives { get; set; }
    public int roomId { get; set; }
    public int xPosition { get; set; }
    public int yPosition { get; set; }
    private Room currentRoom;
    public List<IItem> items;
    public List<Key> keys;
     
    public Player()
    {
        lives = 3;
    }

    // direction == keyinput
    public void Move(string direction)
    {
        int newX = xPosition;
        int newY = yPosition;

        switch (direction)
        {
            case "up": newY--; break;
            case "down": newY++; break;
            case "left": newX--; break;
            case "right": newX++; break;
        }

        if (newX >= 0 && newX < currentRoom.width && newY >= 0 && newY < currentRoom.height)
        {
            xPosition = newX;
            yPosition = newY;
            InteractWithItem();
        }
    }

    public bool CanMoveToNextRoom(string direction, Room[] rooms)
    {
        var relevantConnections = connections.Where(c =>
            c.NORTH ==  roomId || c.SOUTH == roomId ||
            c.WEST == roomId || c.EAST == roomId).ToList();

        foreach (var connection in relevantConnections)
        {
            var coloredDoor = connection.doors.FirstOrDefault(d => d.type == "colored");
            if (coloredDoor != null && !keys.Any(k => k.Color == coloredDoor.color)) return false;

            Room nextRoom;
            switch (direction)
            {
                case "up" when connection.SOUTH == roomId:
                    nextRoom = rooms.First(r => r.Id == connection.NORTH);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    roomId = connection.NORTH;
                    yPosition = nextRoom.height - 1;
                    xPosition = (player.xPosition - (currentRoom.width / 2)) + (nextRoom.width / 2);
                    break;
                case "down" when connection.NORTH == roomId:
                    nextRoom = rooms.First(r => r.Id == connection.SOUTH);
                    MarkDoorAsClosed(connection.doors, "closing gate");

                    roomId = connection.SOUTH;
                    yPosition = 0;
                    xPosition = (player.xPosition - (currentRoom.width / 2)) + (nextRoom.width / 2);
                    break;
                case "left" when connection.EAST == roomId:
                    nextRoom = rooms.First(r => r.Id == connection.WEST);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    roomId = connection.WEST;
                    xPosition = nextRoom.width - 1;
                    player.yPosition = (yPosition - (currentRoom.height / 2)) + (nextRoom.height / 2);
                    break;
                case "right" when connection.WEST == roomId:
                    nextRoom = rooms.First(r => r.Id == connection.EAST);
                    MarkDoorAsClosed(connection.doors, "closing gate");
                    roomId = connection.EAST;
                    xPosition = yPosition = (yPosition - (currentRoom.height / 2)) + (nextRoom.height / 2);
                    break;
                default:
                    continue;
            }

            // Voorkom dat speler buiten de grenzen van de volgende kamer komt
            xPosition = Math.Max(0, Math.Min(xPosition, nextRoom.width - 1));
            yPosition = Math.Max(0, Math.Min(yPosition, nextRoom.height - 1));
            return true;
        }

        return false;
    }

    public bool IsAtDoor(string direction, ConnectionDto[] connections)
    {
        bool atEdge = (direction == "up" && yPosition == 0) ||
                      (direction == "down" && yPosition == currentRoom.height - 1) ||
                      (direction == "left" && xPosition == 0) ||
                      (direction == "right" && xPosition == currentRoom.width - 1);

        if (!atEdge) return false;

        var relevantConnection = connections.FirstOrDefault(c =>
            (direction == "up" && c.SOUTH == currentRoom.Id) ||
            (direction == "down" && c.NORTH == currentRoom.Id) ||
            (direction == "left" && c.EAST == currentRoom.Id) ||
            (direction == "right" && c.WEST == currentRoom.Id));

        if (relevantConnection != null)
        {
            var coloredDoor = relevantConnection.doors.FirstOrDefault(d => d.type == "colored");
            if (coloredDoor != null)
            {
                bool hasMatchingKey = keys.Any(k => k.Color == coloredDoor.color); // door klasse 
                if (!hasMatchingKey) return false;
            }

            var closingGate = relevantConnection.doors.FirstOrDefault(d => d.type == "closing gate");
            if (closingGate != null && closingGate.isClosedPermanently) return false;
        }

        return relevantConnection != null;
    }

    public void InteractWithItem()
    {
        var item = currentRoom.items?.FirstOrDefault(i => i.x == xPosition && i.y == yPosition);
        if (item != null)
        {
            switch (item)
            {
                case BoobyTrap _:
                case DisappearingBoobyTrap _:
                    SubtractLives(1);
                    if (item is DisappearingBoobyTrap) currentRoom.RemoveItem(item);
                    break;
                case Key key:
                    keys.Add(key);
                    currentRoom.RemoveItem(key);
                    break;
                case SankaraStone stone:
                    items.Add(stone);
                    currentRoom.RemoveItem(stone);
                    break;

            }
        }
    }

    // connections vervangen met nieuwe door implenmentatie
    public void InteractWithDoor(string direction)
    {
        bool atEdge = (direction == "up" && yPosition == 0) ||
                      (direction == "down" && yPosition == currentRoom.height - 1) ||
                      (direction == "left" && xPosition == 0) ||
                      (direction == "right" && xPosition == currentRoom.width - 1);

        if (!atEdge) return false;

        var relevantConnection = connections.FirstOrDefault(c =>
            (direction == "up" && c.SOUTH == currentRoom.Id) ||
            (direction == "down" && c.NORTH == currentRoom.Id) ||
            (direction == "left" && c.EAST == currentRoom.Id) ||
            (direction == "right" && c.WEST == currentRoom.Id));

        if (relevantConnection != null)
        {
            var coloredDoor = relevantConnection.doors.FirstOrDefault(d => d.type == "colored");
            if (coloredDoor != null)
            {
                bool hasMatchingKey = collectedKeys.Any(k => k.Color == coloredDoor.color);
                if (!hasMatchingKey) return false;
            }

            var closingGate = relevantConnection.doors.FirstOrDefault(d => d.type == "closing gate");
            if (closingGate != null && closingGate.isClosedPermanently) return false;
        }

        return relevantConnection != null;
    }

    private void CheckWinCondition()
    {
        
    }

    public bool hasRequiredKey(Color color)
    {
        foreach (IItem item in items)
        {
            if (item is Key key && key.Color == color)
            {
                return true;
            }
        }
        
        return false;
    }

    public void SubtractLives(int amount)
    {
        lives = Math.Max(0, lives - amount);
    }
    
    private void EndGame()
    {
        
    }
}