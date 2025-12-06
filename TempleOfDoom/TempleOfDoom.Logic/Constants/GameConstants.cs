namespace TempleOfDoom.Logic.Constants;

public static class GameRules
{
    public const int WinningStoneCount = 5;
    public const int LosingLivesCount = 0;
}

public static class Commands
{
    public const string Shoot = "shoot";
}

public static class Direction
{
    public const string Up = "up";
    public const string Down = "down";
    public const string Left = "left";
    public const string Right = "right";
}

public static class CardinalDirection
{
    public const string North = "north";
    public const string East = "east";
    public const string South = "south";
    public const string West = "west";
}

public static class RelativeDirection
{
    public const string Upper = "upper";
    public const string Lower = "lower";
}


public static class EnemyDirection
{
    public const string Horizontal = "horizontal";
    public const string Vertical = "vertical";
}

public static class GameColors
{
    public const string Red = "red";
    public const string Green = "green";
}

public static class ItemTypes
{
    public const string BoobyTrap = "boobytrap";
    public const string DisappearingBoobyTrap = "disappearing boobytrap";
    public const string SankaraStone = "sankara stone";
    public const string Key = "key";
    public const string PressurePlate = "pressure plate";
}

public static class DoorTypes
{
    public const string Colored = "colored";
    public const string Toggle = "toggle";
    public const string ClosingGate = "closing gate";
    public const string OpenOnStonesInRoom = "open on stones in room";
    public const string OpenOnOdd = "open on odd";
    public const string Ladder = "ladder";
}

public static class SpecialFloorTilesTypes
{
    public const string Ice = "ice";
}