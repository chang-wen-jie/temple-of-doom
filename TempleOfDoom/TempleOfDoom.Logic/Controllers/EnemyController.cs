using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Controllers;

public static class EnemyController
{
    public static Dictionary<ILiving, (int x, int y)> MoveAll(Room room)
    {
        var oldPositions = new Dictionary<ILiving, (int x, int y)>();

        foreach (var enemy in room.Enemies)
        {
            oldPositions[enemy] = GetCoordinates(enemy);

            if (enemy is IMovable movableEnemy)
            {
                movableEnemy.Move();
            }
        }

        return oldPositions;
    }

    public static void CheckCollisions(Room room, Player player, int playerOldX, int playerOldY,
        Dictionary<ILiving, (int x, int y)> enemyOldPositions)
    {
        foreach (var enemy in room.Enemies)
        {
            var (enemyNewX, enemyNewY) = GetCoordinates(enemy);

            if (!enemyOldPositions.TryGetValue(enemy, out var oldPos)) continue;

            var (enemyOldX, enemyOldY) = oldPos;
            var sameTile = player.X == enemyNewX && player.Y == enemyNewY;
            var swappedTiles = player.X == enemyOldX && player.Y == enemyOldY && enemyNewX == playerOldX &&
                          enemyNewY == playerOldY;

            if (sameTile || swappedTiles) player.TakeDamage(Rules.DamageValue);
        }
    }

    public static void HandleAttack(Room room, Player player)
    {
        var attackZones = new List<(int x, int y)>
        {
            (player.X, player.Y - 1),
            (player.X, player.Y + 1),
            (player.X - 1, player.Y),
            (player.X + 1, player.Y)
        };

        foreach (var enemy in room.Enemies)
        {
            var (enemyX, enemyY) = GetCoordinates(enemy);

            foreach (var (attackZoneX, attackZoneY) in attackZones)
            {
                if (enemyX != attackZoneX || enemyY != attackZoneY) continue;
                enemy.DoDamage(Rules.DamageValue);

                // DLL zet CurrentField en vijand op null; opnieuw toewijzen
                if (enemy is not Enemy { NumberOfLives: > Rules.LosingLivesCount } survivingEnemy) continue;
                var field = room.GetField(enemyX, enemyY);
                survivingEnemy.CurrentField = field;

                if (field != null) field.Item = survivingEnemy;
            }
        }
    }

    public static void RemoveDead(Room room)
    {
        var deadEnemies = room.Enemies.Where(e => e.NumberOfLives <= Rules.LosingLivesCount).ToList();

        foreach (var enemy in deadEnemies) room.RemoveEnemy(enemy);
    }

    private static (int x, int y) GetCoordinates(ILiving enemy)
    {
        if (enemy is Enemy baseEnemy)
        {
            return (baseEnemy.CurrentXLocation, baseEnemy.CurrentYLocation);
        }
    
        return (-1, -1);
    }
}