using TempleOfDoom.Logic.Events;

namespace TempleOfDoom.Logic.Models.Items;

public class PressurePlate : IItem, ISubject
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int XPos { get; set; }
    public int YPos { get; set; }
    public bool IsLootable => false;
    private List<IObserver> _observers = new();

    public void Interact(Player player)
    {
        notify();
    }

    public void attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void notify()
    {
        foreach (IObserver observer in _observers)
        {
            observer.update(this);
        }
    }
}