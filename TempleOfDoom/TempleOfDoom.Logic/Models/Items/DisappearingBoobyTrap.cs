using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Items;

public class DisappearingBoobyTrap : BaseItem
{
    public int Damage { get; init; }

    public override void Interact(Player player)
    {
        player.TakeDamage(Damage);
        NotifyDepleted();
    }
}