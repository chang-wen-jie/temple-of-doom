namespace TempleOfDoom.Logic.Models.Doors;

public class ColoredDecorator(Door wrappee, string requiredColor) : Decorator(wrappee)
{
    private readonly string _requiredColor = requiredColor;
    
    public override bool CanEnter(Player player)
    {
        return base.CanEnter(player) && player.HasKeyWithColor(_requiredColor);
    }
}