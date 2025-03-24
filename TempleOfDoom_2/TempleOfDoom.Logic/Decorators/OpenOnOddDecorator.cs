//namespace TempleOfDoom.Logic.Decorators;

//public class OpenOnOddDecorator : IDoor
//{
//    private IDoor _wrappee;
//    public bool IsOpen { get; }
//    public Direction Direction => _wrappee.Direction;
//    public Room firstRoom => _wrappee.firstRoom;
//    public Room secondRoom => _wrappee.secondRoom;
//    private Player Player;
    
//    public OpenOnOddDecorator(IDoor wrappee, Player player)
//    {
//        this._wrappee = wrappee;
//        this.IsOpen = false;
//        this.Player = player;
//    }

//    public void Open()
//    {
//        if (Player.lives % 2 == 1)
//        {
//            _wrappee.Open();
//        }
//    }
    
//    public void Close()
//    {
//        _wrappee.Close();
//    }
    
//    public void Update()
//    {
//        throw new NotImplementedException();
//    }
//}