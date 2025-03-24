namespace TempleOfDoom.Logic.Models;

public class Player
{
    public int startRoomId { get; private set; }
    public int startXPos { get; private set; }
    public int startYPos { get; private set; }
    public int lives { get; private set; }
    
    public Player(int startRoomId, int startXPos, int startYPos, int lives)
    {
        this.startRoomId = startRoomId;
        this.startXPos = startXPos;
        this.startYPos = startYPos;
        this.lives = lives;
    }

    public void decreaseLives(int amount)
    {
        if (amount <= lives) lives -= amount;
    }
}