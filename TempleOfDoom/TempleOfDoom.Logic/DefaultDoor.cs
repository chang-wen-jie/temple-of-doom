namespace TempleOfDoom.Logic;

public class DefaultDoor : IDoor
{
    public int Id { get; }
    public bool IsOpen { get; set; }
    public Direction Direction { get; }
    public Room firstRoom { get; }
    public Room secondRoom { get; }


    public DefaultDoor(int id, Direction direction, Room firstRoom, Room secondRoom)
    {
        Id = id;
        IsOpen = false;
        Direction = direction;
        this.firstRoom = firstRoom;
        this.secondRoom = secondRoom;
    }
    
    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Open()
    {
        this.IsOpen = true;
    }

    public void Close()
    {
        this.IsOpen = false;
    }
}