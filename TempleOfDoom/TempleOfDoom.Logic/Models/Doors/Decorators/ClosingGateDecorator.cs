namespace TempleOfDoom.Logic.Models.Doors.Decorators;

public class ClosingGateDecorator(Door wrappee) : Decorator(wrappee)
{
    public override void OnEnter()
    {
        Close();
        base.OnEnter();
    }
}