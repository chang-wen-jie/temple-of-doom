namespace TempleOfDoom.Logic;

public class PressurePlate : Publisher, IItem
{
    public bool IsLootable { get; set; }
    public void Interact()
    {
        Notify();
    }

    public override void Attach(IObserver observer)
    {
        base.Observers.Add(observer);
    }

    public override void Detach(IObserver observer)
    {
        base.Observers.Remove(observer);
    }

    protected override void Notify()
    {
        foreach (IObserver observer in base.Observers)
        {
            observer.Update();
        }
    }
}