namespace TempleOfDoom.Logic.Models.Doors.Decorators;

public class OpenOnStonesInRoomDecorator(Door wrappee, Func<int> getStoneCount, int requiredStones) : Decorator(wrappee)
{
    public override void Open()
    {
        var currentStones = getStoneCount();

        if (currentStones >= requiredStones) base.Open();
    }
}