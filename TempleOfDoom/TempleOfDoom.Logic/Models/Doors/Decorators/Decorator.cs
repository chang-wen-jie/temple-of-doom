using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Doors.Decorators;

public abstract class Decorator(Door wrappee) : Door
{
    private Door Wrappee { get; } = wrappee;

    public override int X => Wrappee.X;
    public override int Y => Wrappee.Y;
    public override int TargetRoomId => Wrappee.TargetRoomId;
    public override string? DoorType => Wrappee.DoorType;
    public override string? Color => Wrappee.Color;

    public override Door? TwinDoor
    {
        get => Wrappee.TwinDoor;
        set => Wrappee.TwinDoor = value;
    }

    public override bool IsOpen => Wrappee.IsOpen;

    public override void Open()
    {
        Wrappee.Open();
    }

    public override void Close()
    {
        Wrappee.Close();
    }

    public override bool CanEnter(Player player)
    {
        return Wrappee.CanEnter(player);
    }

    public override void OnEnter()
    {
        Wrappee.OnEnter();
    }
}