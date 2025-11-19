namespace TempleOfDoom.Logic.Models.Doors;

public class OpenOnOddDecorator(Door wrappee) : Decorator(wrappee)
{
    public override bool CanEnter(Player player)
    {
        if (!base.CanEnter(player)) return false;

        return player.Lives % 2 != 0;
    }
}