//namespace TempleOfDoom.Logic;

//public class DefaultDoor : IDoor
//{
//    public int Id { get; }
//    public string Type { get; }
//    public bool IsOpen { get; set; }
//    public Direction Direction { get; }
//    public Room firstRoom { get; }
//    public Room secondRoom { get; }


//    public DefaultDoor(int id, Direction direction, Room firstRoom, Room secondRoom)
//    {
//        IsOpen = true;
//        rooms = new Dictionary<Direction, Room>();
//    }
    
//    public void Update()
//    {
//        throw new NotImplementedException();
//    }

//    public void Open()
//    {
//        this.IsOpen = true;
//    }

//    public void Close()
//    {
//        this.IsOpen = false;
//    }

//    public void addRoom(Direction direction, Room room)
//    {
//        if (rooms.Count == 2)
//        {
//            throw new InvalidOperationException("Door already has two rooms");
//        }
        
//        rooms.Add(direction, room);
//    }
//}