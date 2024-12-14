using TempleOfDoom.Data;
using TempleOfDoom.Logic.Decorators;

namespace TempleOfDoom.Logic.Factories;

public class DoorFactory
{
    public IDoor createDoor(DoorDto doorDto, Player player)
    {
        string type = doorDto.type;

        switch (type)
        {
            case "colored":
                return new ColoredDecorator(new DefaultDoor(), doorDto.color, player);
            case "toggle":
                return new ToggleDecorator(new DefaultDoor());
            case "closing gate":
                return new ClosingDecorator(new DefaultDoor());
            case "open on odd":
                return new OpenOnOddDecorator(new DefaultDoor(), player);
            case "open on stones in room":
                return new OpenOnStonesInRoomDecorator(new DefaultDoor(), doorDto.no_of_stones, player);
        }
    }
}