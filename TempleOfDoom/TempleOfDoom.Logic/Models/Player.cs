using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models;

public class Player
{
    public int StartRoomId { get; private set; }
    public int StartXPos { get; private set; }
    public int StartYPos { get; private set; }
    private int Lives { get; set; }
    private List<IItem> Items { get; set; }
    
    public Player(int startRoomId, int startXPos, int startYPos, int lives)
    {
        this.StartRoomId = startRoomId;
        this.StartXPos = startXPos;
        this.StartYPos = startYPos;
        this.Lives = lives;
        Items = new();
    }

    public void decreaseLives(int amount)
    {
        if (amount <= Lives) Lives -= amount;
    }

    public void addItem(IItem item)
    {
        Items.Add(item);
    }
    
    public IItem removeItem(IItem item)
    {
        return Items.Remove(item) ? item : null;
    }
    
    public bool hasItem(IItem item)
    {
        return Items.Contains(item);
    }
}