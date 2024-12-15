namespace TempleOfDoom.Data
{
    public class GameDataDTO
    {
        public Room[] rooms { get; set; }
        public Connection[] connections { get; set; }
        public Player player { get; set; }
    }

    public class Player
    {
        public int startRoomId { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public int lives { get; set; }
    }

    public class Room
    {
        public int id { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Item[] items { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public int damage { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string color { get; set; }
    }

    public class Connection
    {
        public int NORTH { get; set; }
        public int SOUTH { get; set; }
        public Door[] doors { get; set; }
        public int WEST { get; set; }
        public int EAST { get; set; }
    }

    public class Door
    {
        public string type { get; set; }
        public string color { get; set; }
        public int no_of_stones { get; set; }
        public bool isClosedPermanently { get; set; }
    }
}
