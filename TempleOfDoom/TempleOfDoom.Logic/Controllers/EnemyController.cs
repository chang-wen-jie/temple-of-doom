using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Models;

namespace TempleOfDoom.Logic.Controllers;

public static class EnemyController
{
    public static Dictionary<ILiving, (int x, int y)> MoveAll(Room room)
    {
        var oldPositions = new Dictionary<ILiving, (int x, int y)>();

        foreach (var enemy in room.Enemies)
        {
            oldPositions[enemy] = GetCoordinates(enemy);

            switch (enemy)
            {
                case HorizontallyMovingEnemy hEnemy: hEnemy.Move(); break;
                case VerticallyMovingEnemy vEnemy: vEnemy.Move(); break;
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
            var swapped = player.X == enemyOldX && player.Y == enemyOldY && enemyNewX == playerOldX &&
                          enemyNewY == playerOldY;

            if (sameTile || swapped) player.TakeDamage(1);
        }
    }

    public static void HandleAttack(Room room, Player player)
    {
        // Cellen rondom speler constateren
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
                if (enemyX == attackZoneX && enemyY == attackZoneY)
                    enemy.DoDamage(1);
        }
    }

    public static void RemoveDead(Room room)
    {
        var deadEnemies = room.Enemies.Where(e => e.NumberOfLives <= 0).ToList();

        foreach (var enemy in deadEnemies) room.RemoveEnemy(enemy);
    }

    private static (int x, int y) GetCoordinates(ILiving enemy)
    {
        return enemy switch
        {
            HorizontallyMovingEnemy hEnemy => (hEnemy.CurrentXLocation, hEnemy.CurrentYLocation),
            VerticallyMovingEnemy vEnemy => (vEnemy.CurrentXLocation, vEnemy.CurrentYLocation),
            _ => (-1, -1)
        };
    }
}