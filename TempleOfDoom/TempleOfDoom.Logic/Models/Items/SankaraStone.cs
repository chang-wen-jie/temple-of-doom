using TempleOfDoom.Logic.Models.Entities;

namespace TempleOfDoom.Logic.Models.Items;

public class SankaraStone : BaseItem
{
    public override bool IsLootable => true;

    public override void Interact(Player player)
    {
        player.AddItem(this);
        NotifyDepleted();
    }
}