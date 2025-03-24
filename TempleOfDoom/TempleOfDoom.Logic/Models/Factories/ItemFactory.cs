using TempleOfDoom.Data;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models.Factories;

public static class ItemFactory
{
    public static IItem CreateItem(ItemDto itemDto)
    {
        return itemDto.type switch
        {
            "boobytrap" => new BoobyTrap()
            {
                Type = itemDto.type,
                Damage = itemDto.damage,
                XPos = itemDto.x,
                YPos = itemDto.y,
            },
            "disappearing boobytrap" => new DisappearingBoobyTrap()
            {
                Type = itemDto.type,
                Damage = itemDto.damage,
                XPos = itemDto.x,
                YPos = itemDto.y,
            },
            "sankara stone" => new SankaraStone()
            {
                Type = itemDto.type,
                XPos = itemDto.x,
                YPos = itemDto.y,
            },
            "key" => new Key()
            {
                Type = itemDto.type,
                Color = itemDto.color,
                XPos = itemDto.x,
                YPos = itemDto.y,
            },
            "pressure plate" => new PressurePlate()
            {
                Type = itemDto.type,
                XPos = itemDto.x,
                YPos = itemDto.y,
            },
            _ => throw new ArgumentException("Invalid item type")
        };
    }
}