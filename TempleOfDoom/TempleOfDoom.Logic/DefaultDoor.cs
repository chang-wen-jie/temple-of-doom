namespace TempleOfDoom.Logic;

public class DefaultDoor : IDoor
{
    public int Id { get; }
    public string Type { get; }
    public bool IsOpen { get; set; }


    public DefaultDoor()
    {
        IsOpen = false;
    }
    
    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Open()
    {
        this.IsOpen = true;
    }

    public void Close()
    {
        this.IsOpen = false;
    }
}