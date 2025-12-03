using TempleOfDoom.Data;
using TempleOfDoom.Logic.Constants;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ItemFactory
{
    public static IItem CreateItem(ItemDto itemDto)
    {
        return itemDto.Type switch
        {
            ItemTypes.BoobyTrap => new BoobyTrap
            {
                Damage = itemDto.Damage,
                XPos = itemDto.X,
                YPos = itemDto.Y
            },
            ItemTypes.DisappearingBoobyTrap => new DisappearingBoobyTrap
            {
                Damage = itemDto.Damage,
                XPos = itemDto.X,
                YPos = itemDto.Y
            },
            ItemTypes.SankaraStone => new SankaraStone
            {
                XPos = itemDto.X,
                YPos = itemDto.Y
            },
            ItemTypes.Key => new Key
            {
                Color = itemDto.Color,
                XPos = itemDto.X,
                YPos = itemDto.Y
            },
            ItemTypes.PressurePlate => new PressurePlate
            {
                XPos = itemDto.X,
                YPos = itemDto.Y
            },
            _ => throw new ArgumentException("Ongeldige ItemType")
        };
    }
}