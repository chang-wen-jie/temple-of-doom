using System.Text.Json;
using TempleOfDoom.Data;

namespace TempleOfDoom.Logic.Services
{
    public class GameDataService
    {
        public static GameDataDTO LoadGameData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            GameDataDTO gameData = JsonSerializer.Deserialize<GameDataDTO>(json) ?? throw new Exception("Kan gameData.json niet inlezen");
            return gameData;
        }

        public static Player CreatePlayer(PlayerDto player)
        {
            return new Player
            {
                roomId = player.startRoomId,
                xPosition = player.startX,
                yPosition = player.startY,
                lives = player.lives,
            };
        }

        public static Room CreateRoom(RoomDto roomDto)
        {
            return new Room
            {
                Id = roomDto.id,
                Type = roomDto.type,
                width = roomDto.width,
                height = roomDto.height,
                items = (roomDto.items ?? []).Select(i => new ItemDto
                {
                    type = i.type,
                    damage = i.damage,
                    x = i.x,
                    y = i.y,
                    color = i.color,
                }).ToArray(),
            };
        }

        public static ItemDto CreateItem(ItemDto itemDto)
        {
            return new ItemDto
            {

            };
        }

        public static ConnectionDto CreateConnection(ConnectionDto connectionDTO)
        {
            return new ConnectionDto
            {
                NORTH = connectionDTO.NORTH,
                SOUTH = connectionDTO.SOUTH,
                EAST = connectionDTO.EAST,
                WEST = connectionDTO.WEST,
                doors = connectionDTO.doors.Select(d => CreateDoor(d)).ToArray(),
            };
        }

        public static IDoor CreateDoor(DoorDto doorDTO)
        {
            return new IDoor
            {
                Type = doorDTO.Type,
                color = doorDTO.color,
                no_of_stones = doorDTO.no_of_stones,
            };
        }
    }
}
