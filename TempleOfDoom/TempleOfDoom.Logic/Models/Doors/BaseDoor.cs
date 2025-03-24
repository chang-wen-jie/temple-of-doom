namespace TempleOfDoom.Logic.Models.Doors;

public class BaseDoor : Door
{
    public bool isOpen { get; private set; }

    public override void Open()
    {
        this.isOpen = true;
    }

    public override void Close()
    {
        this.isOpen = false;
    }
}