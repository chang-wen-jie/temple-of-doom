namespace TempleOfDoom.Logic.Decorators;

public class ClosingDecorator : IDoor
{
    private IDoor _wrappee;
    public int Id => _wrappee.Id;
    public bool IsOpen => _wrappee.IsOpen;
    public Direction Direction => _wrappee.Direction;
    public Room firstRoom => _wrappee.firstRoom;
    public Room secondRoom => _wrappee.secondRoom;
    private bool _hasBeenOpened;
    
    public ClosingDecorator(IDoor wrappee)
    {
        this._wrappee = wrappee;
        this._hasBeenOpened = false;
    }
    
    public void Open()
    {
        if (!_hasBeenOpened)
        {
            _wrappee.Open();
            _hasBeenOpened = true;
        }
        else
        {
            _wrappee.Close();
        }
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
    
    public void Update()
    {
        throw new NotImplementedException();
    }
}