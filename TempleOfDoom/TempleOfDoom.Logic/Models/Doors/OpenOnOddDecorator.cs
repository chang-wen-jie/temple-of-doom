namespace TempleOfDoom.Logic.Models.Doors;

public class OpenOnOddDecorator(Door wrappee) : Decorator(wrappee)
{
    public void Open(int numberOfRemainingLives)
    {
        if (numberOfRemainingLives % 2 != 0)
        {
            base.Open();
        }
    }
    
    public void Close()
    {
        base.Close();
    }
}