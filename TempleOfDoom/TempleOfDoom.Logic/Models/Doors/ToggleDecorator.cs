using TempleOfDoom.Logic.Events;

namespace TempleOfDoom.Logic.Models.Doors;

public class ToggleDecorator(Door wrappee) : Decorator(wrappee), IObserver
{
    private void Open()
    {
        base.Open();
    }
    
    private void Close()
    {
        base.Close();
    }

    public void update(ISubject subject)
    {
        Open();
    }
}