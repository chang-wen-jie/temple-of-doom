using TempleOfDoom.Logic.Events;
using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Items;

public class PressurePlate : BaseItem, ISubject
{
    private readonly List<IObserver> _observers = [];

    public override void Interact(Player player)
    {
        Notify();
    }

    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers) observer.Update();
    }
}