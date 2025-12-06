using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Items;

public class Key : BaseItem
{
    public string? Color { get; init; }
    public override bool IsLootable => true;

    public override void Interact(Player player)
    {
        player.AddItem(this);
        NotifyDepleted();
    }
}