//using TempleOfDoom.Logic.Enums;

//namespace TempleOfDoom.Logic.Decorators;

//public class ColoredDecorator : IDoor
//{
//    private IDoor _wrappee;
//    public bool IsOpen { get; }
//    public Direction Direction => _wrappee.Direction;
//    public Room firstRoom => _wrappee.firstRoom;
//    public Room secondRoom => _wrappee.secondRoom;
//    private Color _color;
//    private Player _player;
    
//    public ColoredDecorator(IDoor wrappee, string color, Player player)
//    {
//        this._wrappee = wrappee;
//        this.IsOpen = false;
//        this._color = Enum.Parse<Color>(color, true);
//        this._player = player;
//    }
    
//    public void Open()
//    {
//        if (_player.hasRequiredKey(this._color))
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