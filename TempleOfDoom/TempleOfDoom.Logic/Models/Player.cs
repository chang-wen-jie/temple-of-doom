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
        
        public IItem RemoveItem(IItem item)
        {
            return Items.Remove(item) ? item : null;
        }
        
        public bool HasItem(IItem item)
        {
            return Items.Contains(item);
        }
        
        public IEnumerable<IItem> GetItems()
        {
            return Items.AsReadOnly();
        }
        
        public bool MovePlayer(Room currentRoom, string direction)
        {
            var (newX, newY) = CalculateNewPosition(direction);

            if (!IsValidMove(currentRoom, newX, newY)) 
                return false;

            StartXPos = newX;
            StartYPos = newY;
            
            HandleItemInteraction(currentRoom);
            return true;
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

        private static bool IsValidMove(Room currentRoom, int newX, int newY)
        {
            return newX >= 0 && newX < currentRoom.Width && 
                   newY >= 0 && newY < currentRoom.Height;
        }

        private void HandleItemInteraction(Room currentRoom)
        {
            var item = currentRoom.Items?
                .FirstOrDefault(i => i.XPos == StartXPos && i.YPos == StartYPos);

            if (item == null) return;
            item.Interact(this);

            if (item is DisappearingBoobyTrap { ShouldBeRemoved: true } disappearingTrap)
            {
                currentRoom.Items?.Remove(disappearingTrap);
            }
        }
    }
}