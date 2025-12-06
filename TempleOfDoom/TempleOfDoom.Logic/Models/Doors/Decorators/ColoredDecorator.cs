using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Doors.Decorators;

public class ColoredDecorator(Door wrappee, string? requiredColor) : Decorator(wrappee)
{
    public override bool CanEnter(Player player)
    {
        return base.CanEnter(player) && player.HasKeyWithColor(requiredColor);
    }
}