using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models
{
    public class Player(int startRoomId, int startXPos, int startYPos, int lives)
    {
        public int StartRoomId { get; private set; } = startRoomId;
        public int StartXPos { get; private set; } = startXPos;
        public int StartYPos { get; private set; } = startYPos;
        public int Lives { get; private set; } = lives;
        private List<IItem> Items { get; } = [];

        public void DecreaseLives(int amount)
        {
            Lives = Math.Max(0, Lives - amount);
        }

        public void AddItem(IItem item)
        {
            if (item.IsLootable)
            {
                Items.Add(item);
            }
        }
        
        public IEnumerable<IItem> GetItems()
        {
            return Items.AsReadOnly();
        }
        
        public bool HasKeyWithColor(string requiredColor)
        {
            if (string.IsNullOrEmpty(requiredColor)) return false;

            return Items
                .OfType<Key>()
                .Any(k => k.Color.Equals(requiredColor, StringComparison.OrdinalIgnoreCase));
        }
        
        public void Move(Room currentRoom, string direction)
        {
            var (newX, newY) = CalculateNewPosition(direction);

            if (!currentRoom.IsPositionValid(newX, newY)) return;

            StartXPos = newX;
            StartYPos = newY;
    
            currentRoom.HandlePlayerEntry(this);
        }

        private (int newX, int newY) CalculateNewPosition(string direction)
        {
            return direction switch
            {
                "up" => (StartXPos, StartYPos - 1),
                "down" => (StartXPos, StartYPos + 1),
                "left" => (StartXPos - 1, StartYPos),
                "right" => (StartXPos + 1, StartYPos),
                _ => (StartXPos, StartYPos)
            };
        }
        
        public void SetRoom(int roomId, int x, int y)
        {
            StartRoomId = roomId;
            StartXPos = x;
            StartYPos = y;
        }
    }
}