using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Data.DTOs;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Level;

namespace TempleOfDoom.Logic.Models.Factories;

public static class EnemyFactory
{
    public static void CreateAndAddEnemies(Room room, EnemyDto[]? enemyDtos)
    {
        if (enemyDtos == null) return;

        foreach (var dto in enemyDtos)
        {
            ILiving? enemy = dto.Type switch
            {
                EnemyDirection.Horizontal => new HorizontallyMovingEnemy(3, dto.X, dto.Y, dto.MinX, dto.MaxX),
                EnemyDirection.Vertical => new VerticallyMovingEnemy(3, dto.X, dto.Y, dto.MinY, dto.MaxY),
                _ => null
            };

            switch (enemy)
            {
                case null:
                    continue;
                case Enemy baseEnemy:
                {
                    var field = room.GetField(dto.X, dto.Y);

                    if (field == null) continue;

                    baseEnemy.CurrentField = field;

                    // Vijand in kamer plaatsen als Item (voor DLL)
                    if (enemy is IPlacable placableEnemy) room.SetPlacable(dto.X, dto.Y, placableEnemy);

                    break;
                }
            }

            enemy.OnDeath += (_, _) => { };
            room.AddEnemy(enemy);
        }
    }
}