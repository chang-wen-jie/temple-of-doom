using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Items;

namespace TempleOfDoom.Logic.Models;

public class Room
{
    public int Id { get; }
    public int Width { get; }
    public int Height { get; }

    private readonly List<IItem> _items = [];
    public IEnumerable<IItem> Items => _items.AsReadOnly();

    private readonly List<Door> _doors = [];
    public IEnumerable<Door> Doors => _doors.AsReadOnly();

    private readonly List<ILiving> _enemies = [];
    public IEnumerable<ILiving> Enemies => _enemies.AsReadOnly();

    private readonly List<SpecialFloorTile> _specialFloorTiles = [];
    public IEnumerable<SpecialFloorTile> SpecialFloorTiles => _specialFloorTiles.AsReadOnly();

    private readonly FieldAdapter[,] _fieldGrid;
    private readonly IPlacable?[,] _itemGrid;

    public Room(int id, int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException($"Negatieve kameroppervlakte; {width}x{height}.");

        Id = id;
        Width = width;
        Height = height;

        _fieldGrid = new FieldAdapter[Height, Width];
        _itemGrid = new IPlacable?[Height, Width];

        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            _fieldGrid[y, x] = new FieldAdapter(this, x, y);
    }

    public void AddItem(IItem item)
    {
        _items.Add(item);
    }

    public void AddEnemy(ILiving enemy)
    {
        _enemies.Add(enemy);
    }

    public void AddDoor(Door door)
    {
        _doors.Add(door);
    }

    public void AddSpecialFloorTile(SpecialFloorTile tile)
    {
        _specialFloorTiles.Add(tile);
    }

    public bool IsWalkable(int x, int y)
    {
        // Constateer grenzen
        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        // Constateer deuren
        var isWall = x == 0 || x == Width - 1 || y == 0 || y == Height - 1;
        return !isWall || Doors.Any(d => d.X == x && d.Y == y);
    }

    public void OnPlayerEnter(Player player)
    {
        var item = Items.FirstOrDefault(i => i.XPos == player.X && i.YPos == player.Y);

        if (item == null) return;

        item.Interact(player);

        if (ShouldRemoveItem(item)) _items.Remove(item);
    }

    public IPlacable? GetItem(int x, int y)
    {
        return IsValidCoordinate(x, y) ? _itemGrid[y, x] : null;
    }

    public void SetItem(int x, int y, IPlacable? item)
    {
        if (IsValidCoordinate(x, y)) _itemGrid[y, x] = item;
    }

    public FieldAdapter? GetField(int x, int y)
    {
        return IsValidCoordinate(x, y) ? _fieldGrid[y, x] : null;
    }

    public bool HasSpecialTile(int x, int y, string type)
    {
        return _specialFloorTiles.Any(t => t.X == x && t.Y == y && t.Type == type);
    }

    private static bool ShouldRemoveItem(IItem item)
    {
        if (item.IsLootable) return true;
        return item is DisappearingBoobyTrap { ShouldBeRemoved: true };
    }

    public void RemoveEnemy(ILiving enemy)
    {
        _enemies.Remove(enemy);

        if (enemy is not (Enemy e and IPlacable placable)) return;
        if (GetItem(e.CurrentXLocation, e.CurrentYLocation) == placable)
            SetItem(e.CurrentXLocation, e.CurrentYLocation, null);
    }

    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}