using TempleOfDoom.Logic.Events;

namespace TempleOfDoom.Logic.Models.Doors;

public class ToggleDecorator(Door wrappee) : Decorator(wrappee), IObserver
{
    public void Update(ISubject subject)
    {
        if (IsOpen) Close();
        else Open();
    }
}