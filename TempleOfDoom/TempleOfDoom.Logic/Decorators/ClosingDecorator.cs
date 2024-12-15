namespace TempleOfDoom.Logic.Decorators;

public class ClosingDecorator : IDoor
{
    private IDoor _wrappee;
    public bool IsOpen { get; }
    public Direction Direction => _wrappee.Direction;
    public Room firstRoom => _wrappee.firstRoom;
    public Room secondRoom => _wrappee.secondRoom;
    private bool _hasBeenOpened;
    
    public ClosingDecorator(IDoor wrappee)
    {
        this._wrappee = wrappee;
        this.IsOpen = false;
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