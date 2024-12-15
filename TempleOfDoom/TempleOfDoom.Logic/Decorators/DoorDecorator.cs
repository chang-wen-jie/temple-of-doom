namespace TempleOfDoom.Logic.Decorators;

public abstract class DoorDecorator : IDoor
{
    public int Id { get; }
    public string Type { get; }
    public bool IsOpen { get; }
    protected IDoor Wrappee;
    
    public DoorDecorator(IDoor wrappee)
    {
        this.Wrappee = wrappee;
    }

    public abstract void Update();

    public void Open()
    {
        Wrappee.Open();
    }

    public void Close()
    {
        Wrappee.Close();
    }
}