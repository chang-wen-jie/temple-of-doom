namespace TempleOfDoom.Logic.Models.Doors;

public class ClosingGateDecorator : Decorator
{
    private Door _wrappee { get; }

    public ClosingGateDecorator(Door wrappee) : base(wrappee)
    {
        _wrappee = wrappee;
        base.Open();
    }

    public void open()
    {
        base.Close();
    }

    public void close()
    {
        base.Close();
    }
}