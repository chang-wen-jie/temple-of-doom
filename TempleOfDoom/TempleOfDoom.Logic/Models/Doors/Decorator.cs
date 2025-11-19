namespace TempleOfDoom.Logic.Models.Doors;

public abstract class Decorator : Door
{
    private Door Wrappee { get; }
    
    protected Decorator(Door wrappee)
    {
        Wrappee = wrappee;
        X = wrappee.X;
        Y = wrappee.Y;
        TargetRoomId = wrappee.TargetRoomId;
        TwinDoor = wrappee.TwinDoor;
        Color = wrappee.Color;
        DoorType = wrappee.DoorType;
    }

    public override bool IsOpen => Wrappee.IsOpen;
    public override void Open() => Wrappee.Open();
    public override void Close() => Wrappee.Close();
    public override bool CanEnter(Player player) => Wrappee.CanEnter(player);
    public override void OnEnter() => Wrappee.OnEnter();
}