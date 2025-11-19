using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class DoorFactory
{
    public static Door DecorateDoor(Door doorToDecorate, DoorDto doorDto)
    {
        return doorDto.type switch
         {
             "colored" => new ColoredDecorator(doorToDecorate, doorDto.color),
             "toggle" => new ToggleDecorator(doorToDecorate),
             "closing gate" => new ClosingGateDecorator(doorToDecorate),
             "open on odd" => new OpenOnOddDecorator(doorToDecorate),
             "open on stones in room" => new OpenOnStonesInRoomDecorator(doorToDecorate, doorDto.no_of_stones),
             _ => doorToDecorate
         };
    }
}