namespace TempleOfDoom.Logic.Decorators;

public class OpenOnStonesInRoomDecorator : DoorDecorator
{
    public int NumberOfStones { get; private set; }
    
    public OpenOnStonesInRoomDecorator(IDoor wrappee) : base(wrappee)
    {
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public new void Open()
    {
        base.Open();
    }

    public new void Close()
    {
        base.Close();
    }
}