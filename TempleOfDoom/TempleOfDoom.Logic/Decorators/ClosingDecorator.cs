namespace TempleOfDoom.Logic.Decorators;

public class ClosingDecorator : DoorDecorator
{
    private bool _hasBeenOpened;
    
    public ClosingDecorator(IDoor wrappee) : base(wrappee)
    {
        this._hasBeenOpened = false;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
    
    public new void Open()
    {
        if (!_hasBeenOpened)
        {
            base.Open();
            _hasBeenOpened = true;
        }
        else
        {
            base.Close();
        }
    }
}