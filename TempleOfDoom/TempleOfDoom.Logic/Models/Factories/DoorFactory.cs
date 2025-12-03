using TempleOfDoom.Data;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;

namespace TempleOfDoom.Logic.Models.Factories;

public static class DoorFactory
{
    private static Door DecorateDoor(Door doorToDecorate, DoorDto doorDto)
    {
        return doorDto.Type switch
        {
            DoorTypes.Colored => new ColoredDecorator(doorToDecorate, doorDto.Color),
            DoorTypes.Toggle => new ToggleDecorator(doorToDecorate),
            DoorTypes.ClosingGate => new ClosingGateDecorator(doorToDecorate),
            DoorTypes.OpenOnOdd => new OpenOnOddDecorator(doorToDecorate),
            DoorTypes.OpenOnStonesInRoom => new OpenOnStonesInRoomDecorator(doorToDecorate, doorDto.NoOfStones),
            _ => doorToDecorate
        };
    }

    public static Door CreateDecoratedDoor(Door baseDoor, DoorDto[]? doorDtos)
    {
        return doorDtos == null ? baseDoor : doorDtos.Aggregate(baseDoor, DecorateDoor);
    }
}