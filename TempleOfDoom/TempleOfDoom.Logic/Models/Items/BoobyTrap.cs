using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Items;

public class BoobyTrap : BaseItem
{
    public int Damage { get; init; }

    public override void Interact(Player player)
    {
        player.TakeDamage(Damage);
    }
}