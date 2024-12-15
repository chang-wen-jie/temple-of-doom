using TempleOfDoom.Data;

namespace TempleOfDoom.Logic.Factories;

public class RoomFactory
{
    public List<Room> CreateRooms(RoomDto[] rooms, ItemFactory itemFactory)
    {
        List<Room> roomList = new List<Room>();

        roomList.AddRange(rooms.Select(room =>
            new Room(room.id, room.height, room.width, itemFactory.CreateItems(room.items))));

        return roomList;
    }
}