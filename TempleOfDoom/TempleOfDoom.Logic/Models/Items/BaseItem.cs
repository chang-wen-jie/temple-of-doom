using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Interfaces;

namespace TempleOfDoom.Logic.Models.Items;

public abstract class BaseItem : IItem
{
    public int XPos { get; init; }
    public int YPos { get; init; }
    public virtual bool IsLootable => false;

    public event EventHandler? OnItemDepleted;

    public abstract void Interact(Player player);

    protected void NotifyDepleted()
    {
        OnItemDepleted?.Invoke(this, EventArgs.Empty);
    }
}