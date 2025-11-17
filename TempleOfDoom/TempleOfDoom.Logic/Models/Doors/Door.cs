namespace TempleOfDoom.Logic.Models.Doors;

public abstract class Door
{
    public int X { get; set; }
    public int Y { get; set; }
    public int TargetRoomId { get; set; }
    public Door? TwinDoor { get; set; }
    public string DoorType { get; set; }
    public string Color { get; set; }

    public abstract bool IsOpen { get; }
    public abstract void Open();
    public abstract void Close();
    public abstract bool CanEnter(Player player);
    
    public char GetSymbol(bool isHorizontalWall)
    {
        return DoorType switch
        {
            "colored" => isHorizontalWall ? '=' : '|',
            "toggle" => '⊥',
            "closing gate" => '∩',
            "open on stones in room" => '?',
            "open on odd" => '!',
            _ => ' '
        };
    }
}