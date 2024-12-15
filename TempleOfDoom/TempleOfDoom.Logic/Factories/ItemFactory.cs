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
    
    public List<IItem> CreateItems(ItemDto[] itemDtos)
    {
        List<IItem> items = new List<IItem>();
        
        foreach (var itemDto in itemDtos)
        {
            string type = itemDto.type;

            switch (type)
            {
                case "boobytrap":
                    items.Add(new BoobyTrap());
                    break;
                case "disappearing boobytrap":
                    items.Add(new DisappearingBoobyTrap());
                    break;
                case "pressure plate":
                    items.Add(new PressurePlate());
                    break;
                case "sankara stone":
                    items.Add(new SankaraStone());
                    break;
                case "key":
                    items.Add(new Key(itemDto.color));
                    break;
            }
        }

        return items;
    }
}