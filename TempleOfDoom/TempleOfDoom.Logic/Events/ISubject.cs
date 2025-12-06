namespace TempleOfDoom.Logic.Events;

public interface ISubject
{
    void Attach(IObserver observer);
}