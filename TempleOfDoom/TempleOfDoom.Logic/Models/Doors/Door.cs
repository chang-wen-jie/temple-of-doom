namespace TempleOfDoom.Logic.Models.Doors;

public abstract class Door
{
    public int X { get; init; }
    public int Y { get; init; }
    public int TargetRoomId { get; init; }
    public string DoorType { get; init; }
    public string Color { get; init; }
    public Door? TwinDoor { get; set; }
    public abstract bool IsOpen { get; }
    public abstract void Open();
    public abstract void Close();
    public abstract bool CanEnter(Player player);
    public virtual void OnEnter() { }
}