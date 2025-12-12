using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Controllers;
using TempleOfDoom.Logic.Models.Items;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Core;

public class GameManager(Level level)
{
    private readonly PlayerMovementController _playerMovementController = new(level);

    public bool HasWon => level.Player.Inventory.OfType<SankaraStone>().Count() >= Rules.WinningStoneCount;
    public bool IsGameOver => level.Player.Lives <= Rules.LosingLivesCount || HasWon;

    public void HandlePlayerInput(string command)
    {
        var player = level.Player;
        var currentRoom = GetCurrentRoom();
        var playerOldX = player.X;
        var playerOldY = player.Y;

        if (command == Commands.Shoot) EnemyController.HandleAttack(currentRoom, player);
        else _playerMovementController.Move(player, currentRoom, command);

        var enemyOldPositions = EnemyController.MoveAll(currentRoom);
        EnemyController.CheckCollisions(currentRoom, player, playerOldX, playerOldY, enemyOldPositions);
        EnemyController.RemoveDead(currentRoom);
    }

    public Room GetCurrentRoom()
    {
        var room = level.GetRoom(level.Player.CurrentRoomId);
        return room ??
               throw new InvalidOperationException(
                   $"Speler bevindt zich in een ongeldige kamernummer: {level.Player.CurrentRoomId}");
    }
}