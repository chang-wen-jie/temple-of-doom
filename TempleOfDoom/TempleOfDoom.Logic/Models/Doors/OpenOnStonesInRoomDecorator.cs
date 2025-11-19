namespace TempleOfDoom.Logic.Models.Doors;

public class OpenOnStonesInRoomDecorator(Door wrappee, int requiredStones) : Decorator(wrappee)
{
    private Door _wrappee { get; } = wrappee;
    private int _requiredStones { get; } = requiredStones;

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