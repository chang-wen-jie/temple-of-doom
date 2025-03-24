using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models;

public class Room
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<IItem> Items { get; set; }
}