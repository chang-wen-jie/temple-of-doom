namespace TempleOfDoom.Logic.Models.Doors;

public class ColoredDecorator(Door wrappee, string color) : Decorator(wrappee)
{
    private string color { get; } = color;
    
    public void Open(string color)
    {
        if (color == this.color)
        {
            base.Open();
        }
    }

    public override void Close()
    {
        base.Open();
    }
}