namespace TempleOfDoom.Logic;

public interface IDoor : IObserver
{
    int Id { get; }
    string Type { get; }
    bool IsOpen { get; }
    
    void Open();
    void Close();
}