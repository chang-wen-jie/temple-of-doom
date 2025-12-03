using TempleOfDoom.Data;

namespace TempleOfDoom.Logic.Models.Factories;

public static class RoomFactory
{
    public static Room CreateRoom(RoomDto roomDto)
    {
        var room = new Room(
            roomDto.Id,
            roomDto.Width,
            roomDto.Height
        );

        if (roomDto.Items != null)
            foreach (var itemDto in roomDto.Items)
            {
                var item = ItemFactory.CreateItem(itemDto);
                room.AddItem(item);
            }

        if (roomDto.SpecialFloorTiles == null) return room;
        foreach (var tileDto in roomDto.SpecialFloorTiles)
            room.AddSpecialFloorTile(new SpecialFloorTile
            {
                Type = tileDto.Type,
                X = tileDto.X,
                Y = tileDto.Y
            });

        return room;
    }
}