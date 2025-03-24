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

        public static PlayerDto CreatePlayer(PlayerDto player)
        {
            return new PlayerDto
            {
                startRoomId = player.startRoomId,
                startX = player.startX,
                startY = player.startY,
                lives = player.lives,
            };
        }

        public static RoomDto CreateRoom(RoomDto room)
        {
            return new RoomDto
            {
                id = room.id,
                type = room.type,
                width = room.width,
                height = room.height,
                items = (room.items ?? []).Select(i => new ItemDto
                {
                    type = i.type,
                    damage = i.damage,
                    x = i.x,
                    y = i.y,
                    color = i.color,
                }).ToArray(),
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

        public static DoorDto CreateDoor(DoorDto doorDTO)
        {
            return new DoorDto
            {
                type = doorDTO.type,
                color = doorDTO.color,
                no_of_stones = doorDTO.no_of_stones,
            };
        }
    }
}
