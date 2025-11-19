using TempleOfDoom.Logic.Events;

namespace TempleOfDoom.Logic.Models.Items;

public class PressurePlate : IItem, ISubject
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => false;
    private readonly List<IObserver> _observers = [];

    public void Interact(Player player)
    {
        Notify();
    }

    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}