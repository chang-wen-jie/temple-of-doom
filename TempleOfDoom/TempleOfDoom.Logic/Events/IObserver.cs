namespace TempleOfDoom.Logic.Events;

public interface IObserver
{
    void update(ISubject subject);
}