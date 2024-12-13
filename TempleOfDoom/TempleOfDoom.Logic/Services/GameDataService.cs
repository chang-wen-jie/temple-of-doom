using System.Text.Json;
using TempleOfDoom.Data;

namespace TempleOfDoom.Logic.Services
{
    public class GameDataService
    {
        public static GameDataDTO LoadGameData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            GameDataDTO? gameData = JsonSerializer.Deserialize<GameDataDTO>(json) ?? throw new Exception("Kan gameData.json niet inlezen");

            return gameData;
        }

        public static Player CreatePlayer(GameDataDTO gameData)
        {
            Player newPlayer = new()
            {
                startRoomId = gameData.player.startRoomId,
                startX = gameData.player.startX,
                startY = gameData.player.startY,
                lives = gameData.player.lives,
            };

            return newPlayer;
        }

        public static Room CreateRoom(GameDataDTO gameData, int roomId)
        {
            Room room = gameData.rooms.FirstOrDefault(r => r.id == roomId) ?? throw new Exception($"Room ID {roomId} bestaat niet");

            Room newRoom = new()
            {
                id = room.id,
                type = room.type,
                width = room.width,
                height = room.height,
                items = (room.items ?? []).Select(i => new Item
                {
                    type = i.type,
                    damage = i.damage,
                    x = i.x,
                    y = i.y,
                    color = i.color,
                }).ToArray()
            };

            return newRoom;
        }

        public static Connection CreateConnection(GameDataDTO gameData, int northRoomId, int southRoomId)
        {
            var connection = gameData.connections.FirstOrDefault(c =>
                (c.NORTH == northRoomId && c.SOUTH == southRoomId) ||
                (c.NORTH == southRoomId && c.SOUTH == northRoomId) ||
                (c.WEST == northRoomId && c.EAST == southRoomId) ||
                (c.WEST == southRoomId && c.EAST == northRoomId)) ??
                throw new Exception($"Geen connectie tussen Rooms IDS {northRoomId} en {southRoomId}");

            Connection newConnection = new()
            {
                NORTH = connection.NORTH,
                SOUTH = connection.SOUTH,
                WEST = connection.WEST,
                EAST = connection.EAST,
                doors = connection.doors.Select(d => new Door
                {
                    type = d.type,
                    color = d.color,
                    no_of_stones = d.no_of_stones,
                }).ToArray(),
            };

            return newConnection;
        }
    }
}
