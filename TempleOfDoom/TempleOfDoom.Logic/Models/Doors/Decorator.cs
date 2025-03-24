namespace TempleOfDoom.Logic.Models.Doors;

public abstract class Decorator(Door wrappee) : Door
{
    private Door _wrappee { get; set; } = wrappee;

    public override void Open()
    {
        _wrappee.Open();
    }

    public override void Close()
    {
        _wrappee.Close();
    }
}