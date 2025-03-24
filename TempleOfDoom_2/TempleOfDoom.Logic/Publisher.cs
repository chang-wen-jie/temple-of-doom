namespace TempleOfDoom.Logic;

public abstract class Publisher
{
    protected List<IObserver> Observers;
    
    public abstract void Attach(IObserver observer);
    public abstract void Detach(IObserver observer);
    protected abstract void Notify();
}