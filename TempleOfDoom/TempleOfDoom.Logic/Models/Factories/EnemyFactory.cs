using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Data;

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
                "horizontal" => new HorizontallyMovingEnemy(3, dto.X, dto.Y, dto.MinX, dto.MaxX),
                "vertical" => new VerticallyMovingEnemy(3, dto.X, dto.Y, dto.MinY, dto.MaxY),
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

                    if (enemy is IPlacable placableEnemy) room.SetItem(dto.X, dto.Y, placableEnemy);

                    break;
                }
            }

            enemy.OnDeath += (_, _) => { };
            room.AddEnemy(enemy);
        }
    }
}