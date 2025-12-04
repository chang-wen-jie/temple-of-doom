using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Controllers;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic;

using Models;

public class GameManager(Level level)
{
    private readonly Level _level = level;
    private readonly PlayerMovementController _playerMovementController = new(level);

    public bool HasWon => _level.Player.Inventory.OfType<SankaraStone>().Count() >= 5;
    public bool IsGameOver => _level.Player.Lives <= 0 || HasWon;

    public void HandlePlayerInput(string command)
    {
        var player = _level.Player;
        var currentRoom = _level.Rooms.First(r => r.Id == player.CurrentRoomId);
        var playerOldX = player.X;
        var playerOldY = player.Y;

        if (command == GameConstants.Shoot) EnemyController.HandleAttack(currentRoom, player);
        else _playerMovementController.Move(player, currentRoom, command);

        var enemyOldPositions = EnemyController.MoveAll(currentRoom);
        EnemyController.CheckCollisions(currentRoom, player, playerOldX, playerOldY, enemyOldPositions);
        EnemyController.RemoveDead(currentRoom);
    }

    public Room GetCurrentRoom()
    {
        return _level.Rooms.First(r => r.Id == _level.Player.CurrentRoomId);
    }
}