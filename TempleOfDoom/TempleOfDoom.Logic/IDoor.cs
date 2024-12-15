namespace TempleOfDoom.Logic;

public interface IDoor : IObserver
{
    bool IsOpen { get; }
    Direction Direction { get; }
    Room firstRoom { get; }
    Room secondRoom { get; }
    
    void Open();
    void Close();
}