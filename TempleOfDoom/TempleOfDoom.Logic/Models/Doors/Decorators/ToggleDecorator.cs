using TempleOfDoom.Logic.Events;

namespace TempleOfDoom.Logic.Models.Doors.Decorators;

public class ToggleDecorator(Door wrappee) : Decorator(wrappee), IObserver
{
    public void Update()
    {
        if (IsOpen) Close();
        else Open();
    }
}