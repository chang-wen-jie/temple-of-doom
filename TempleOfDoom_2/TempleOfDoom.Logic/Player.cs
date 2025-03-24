//using System.Numerics;
//using TempleOfDoom.Data;
//using TempleOfDoom.Logic.Enums;

//namespace TempleOfDoom.Logic;

//public class Player
//{
//    public int lives { get; set; }
//    public int roomId { get; set; }
//    public int xPosition { get; set; }
//    public int yPosition { get; set; }
//    private Room currentRoom;
//    private List<IItem> items;
     
//    public Player()
//    {
//        lives = 3;
//    }

//    public void InteractWithItem()
//    {

//    }

//    private void CheckWinCondition()
//    {
        
//    }

//    public bool hasRequiredKey(Color color)
//    {
//        foreach (IItem item in items)
//        {
//            if (item is Key key && key.Color == color)
//            {
//                return true;
//            }
//        }
        
//        return false;
//    }
    
//    public int getNumberOfStones()
//    {
//        return items.Count(item => item is SankaraStone);
//    }

//    public void SubtractLives(int amount)
//    {
//        lives = Math.Max(0, lives - amount);
//    }
    
//    private void EndGame()
//    {
        
//    }
//}