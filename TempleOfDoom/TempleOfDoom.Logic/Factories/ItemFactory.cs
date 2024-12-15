using TempleOfDoom.Data;
using TempleOfDoom.Logic.Enums;

namespace TempleOfDoom.Logic.Factories;

public class ItemFactory
{
    public IItem CreateItem(ItemDto itemDto)
    {
        string type = itemDto.type;

        switch (type)
        {
            case "boobytrap":
                return new BoobyTrap();
            case "disappearing boobytrap":
                return new DisappearingBoobyTrap();
            case "pressure plate":
                return new PressurePlate();
            case "sankara stone":
                return new SankaraStone();
            case "key":
                return new Key(itemDto.color);
        }
        
        throw new ArgumentException("Invalid item type");
    }
}