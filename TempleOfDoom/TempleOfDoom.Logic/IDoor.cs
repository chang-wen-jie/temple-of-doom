namespace TempleOfDoom.Logic;

public interface IDoor : IObserver
{
    int Id { get; }
    bool IsOpen { get; }
    Direction Direction { get; }
    Room firstRoom { get; }
    Room secondRoom { get; }
    
    void Open();
    void Close();
}