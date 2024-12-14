namespace TempleOfDoom.Logic.Decorators;

public class ToggleDecorator : DoorDecorator
{
    public ToggleDecorator(IDoor wrappee) : base(wrappee)
    {
    }

    public override void Update()
    {
        if (base.IsOpen)
        {
            base.Close();
        }
        else
        {
            base.Open();
        }
    }
}