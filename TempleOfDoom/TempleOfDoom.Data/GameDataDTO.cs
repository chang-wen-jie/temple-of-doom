namespace TempleOfDoom.Data
{
    public class GameDataDTO
    {
        public RoomDto[] rooms { get; set; }
        public ConnectionDto[] connections { get; set; }
        public PlayerDto player { get; set; }
    }

    public class PlayerDto
    {
        public int startRoomId { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public int lives { get; set; }
    }

    public class RoomDto
    {
        public int id { get; set; }
        public string type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public ItemDto[] items { get; set; }
    }

    public class ItemDto
    {
        public string type { get; set; }
        public int damage { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string color { get; set; }
    }

    public class ConnectionDto
    {
        public int NORTH { get; set; }
        public int SOUTH { get; set; }
        public DoorDto[] doors { get; set; }
        public int WEST { get; set; }
        public int EAST { get; set; }
    }

    public class DoorDto
    {
        public string type { get; set; }
        public string color { get; set; }
        public int no_of_stones { get; set; }
        public bool isClosedPermanently { get; set; }
    }
}
