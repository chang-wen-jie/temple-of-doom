namespace TempleOfDoom.Logic.Models.Doors;

public class OpenOnStonesInRoomDecorator: Decorator
{
    private Door _wrappee { get; }
    private int _requiredStones { get; }
    
    public OpenOnStonesInRoomDecorator(Door wrappee, int requiredStones) : base(wrappee)
    {
        _wrappee = wrappee;
        _requiredStones = requiredStones;
    }

    public void Open(int stonesInRoom)
    {
        if (stonesInRoom == _requiredStones)
        {
            base.Open();
        }
    }
    
    public void Close()
    {
        base.Close();
    }
}