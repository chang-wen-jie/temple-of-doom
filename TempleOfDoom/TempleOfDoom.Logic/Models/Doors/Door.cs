using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Doors;

public abstract class Door
{
    public virtual int X { get; init; }
    public virtual int Y { get; init; }
    public virtual int TargetRoomId { get; init; }
    public virtual string? DoorType { get; init; }
    public virtual string? Color { get; init; }

    // Staat van deur onthouden
    public virtual Door? TwinDoor { get; set; }

    public abstract bool IsOpen { get; }
    public abstract void Open();
    public abstract void Close();
    public abstract bool CanEnter(Player player);

    public virtual void OnEnter()
    {
    }
}