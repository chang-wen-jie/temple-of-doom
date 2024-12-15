namespace TempleOfDoom.Logic.Decorators;

public class OpenOnStonesInRoomDecorator : IDoor
{
    private IDoor _wrappee;
    public bool IsOpen => _wrappee.IsOpen;
    public Direction Direction => _wrappee.Direction;
    public Room firstRoom => _wrappee.firstRoom;
    public Room secondRoom => _wrappee.secondRoom;
    private int RequiredNumberOfStones;
    private Player _player;
    
    public OpenOnStonesInRoomDecorator(IDoor wrappee, int requiredStonesAmount, Player player)
    {
        this._wrappee = wrappee;
        this.RequiredNumberOfStones = requiredStonesAmount;
        this._player = player;
        
    }

    public void Open()
    {
        if (_player.getNumberOfStones() == RequiredNumberOfStones)
        {
            _wrappee.Open();
        }
    }

    public void Close()
    {
        _wrappee.Close();
    }
    
    public void Update()
    {
        throw new NotImplementedException();
    }
}