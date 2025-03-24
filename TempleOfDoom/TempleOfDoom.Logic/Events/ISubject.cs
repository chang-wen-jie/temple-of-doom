namespace TempleOfDoom.Logic.Events;

public interface ISubject
{
    void attach(IObserver observer);
    
    void detach(IObserver observer);
    
    void notify();
}