namespace TempleOfDoom.Logic.Models.Doors;

public class ClosingGateDecorator(Door wrappee) : Decorator(wrappee)
{
    public override void OnEnter()
    {
        Close();
        base.OnEnter();
    }
}