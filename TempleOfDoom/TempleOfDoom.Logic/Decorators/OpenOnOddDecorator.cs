namespace TempleOfDoom.Logic.Decorators;

public class OpenOnOddDecorator : DoorDecorator
{
    private Player Player;
    
    public OpenOnOddDecorator(IDoor wrappee, Player player) : base(wrappee)
    {
        this.Player = player;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public new void Open()
    {
        if (Player.lives % 2 == 1)
        {
            base.Open();
        }
    }
    
    public new void Close()
    {
        base.Close();
    }
}