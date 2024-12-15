using TempleOfDoom.Logic.Enums;

namespace TempleOfDoom.Logic.Decorators;

public class ColoredDecorator : DoorDecorator
{
    private Color _color;
    private Player _player;
    
    public ColoredDecorator(IDoor wrappee, string color, Player player) : base(wrappee)
    {
        this._color = Enum.Parse<Color>(color, true);
        this._player = player;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
    
    public new void Open()
    {
        if (_player.hasRequiredKey(this._color))
        {
            base.Open();
        }
    }
    
    public new void Close()
    {
        base.Close();
    }
}