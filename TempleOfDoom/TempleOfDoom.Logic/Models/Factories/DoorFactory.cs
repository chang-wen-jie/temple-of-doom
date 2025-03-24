using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class DoorFactory
{
    public static Door CreateDoor(DoorDto doorDto)
    {
        var door = new BaseDoor();

        return doorDto.type switch
         {
             "colored" => new ColoredDecorator(door, doorDto.color),
             "toggle" => new ToggleDecorator(door),
             "closing gate" => new ClosingGateDecorator(door),
             "open on odd" => new OpenOnOddDecorator(door),
             "open on stones in room" => new OpenOnStonesInRoomDecorator(door, doorDto.no_of_stones),
             _ => throw new ArgumentException("Invalid door type")
         };
    }
}