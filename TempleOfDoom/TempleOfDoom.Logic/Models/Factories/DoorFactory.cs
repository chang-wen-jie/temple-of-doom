using TempleOfDoom.Data;
using TempleOfDoom.Data.DTOs;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Doors.Decorators;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Models.Factories;

public static class DoorFactory
{
    private static Door DecorateDoor(Door doorToDecorate, DoorDto doorDto, Room room)
    {
        return doorDto.Type switch
        {
            DoorTypes.Colored => new ColoredDecorator(doorToDecorate, doorDto.Color),
            DoorTypes.Toggle => new ToggleDecorator(doorToDecorate),
            DoorTypes.ClosingGate => new ClosingGateDecorator(doorToDecorate),
            DoorTypes.OpenOnOdd => new OpenOnOddDecorator(doorToDecorate),
            DoorTypes.OpenOnStonesInRoom => new OpenOnStonesInRoomDecorator(
                doorToDecorate,
                () => room.Items.OfType<SankaraStone>().Count(),
                doorDto.NoOfStones
            ),
            _ => doorToDecorate
        };
    }

    public static Door CreateDecoratedDoor(Door baseDoor, DoorDto[]? doorDtos, Room room)
    {
        // Lagen Decorators aanbrengen aan deur
        return doorDtos == null
            ? baseDoor
            : doorDtos.Aggregate(baseDoor, (currentDoor, dto) => DecorateDoor(currentDoor, dto, room));
    }
}