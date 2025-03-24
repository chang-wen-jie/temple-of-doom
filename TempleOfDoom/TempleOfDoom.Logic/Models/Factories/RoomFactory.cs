using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Factories;

public static class RoomFactory
{
    public static Room CreateRoom(RoomDto roomDto)
    {
        var room = new Room()
        {
            Id = roomDto.id,
            Type = roomDto.type,
            Width = roomDto.width,
            Height = roomDto.height,
            Items = new List<IItem>()
        };

        
        if (roomDto.items == null)
        {
            return room;
        }
        
        foreach (ItemDto itemDto in roomDto.items)
        {
            room.Items.Add(ItemFactory.CreateItem(itemDto));
        }

        return room;
    }
}