namespace TempleOfDoom.Logic.Decorators;

public class ToggleDecorator : IDoor
{
    private IDoor _wrappee;
    public int Id => _wrappee.Id;
    public bool IsOpen => _wrappee.IsOpen;
    public Direction Direction => _wrappee.Direction;
    public Room firstRoom => _wrappee.firstRoom;
    public Room secondRoom => _wrappee.secondRoom;

    public ToggleDecorator(IDoor wrappee)
    {
        this._wrappee = wrappee;
    }
    
    public void Open()
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        if (_wrappee.IsOpen)
        {
            _wrappee.Close();
        }
        else
        {
            _wrappee.Open();
        }
    }
}