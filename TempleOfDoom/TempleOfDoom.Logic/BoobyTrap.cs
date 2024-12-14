namespace TempleOfDoom.Logic;

public class BoobyTrap : IItem
{
    public bool IsLootable { get; set; }
    
    public BoobyTrap()
    {
        IsLootable = false;
    }
    
    public void Interact()
    {
        throw new NotImplementedException();
    }
}