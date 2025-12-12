using CODE_TempleOfDoom_DownloadableContent;
using TempleOfDoom.Logic.Events;
using TempleOfDoom.Logic.Models.Adapters;
using TempleOfDoom.Logic.Models.Doors;
using TempleOfDoom.Logic.Models.Entities;
using TempleOfDoom.Logic.Models.Interfaces;

namespace TempleOfDoom.Logic.Models.Level;

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
    private readonly IPlacable?[,] _placableGrid;

    public Room(int id, int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentException($"Negatieve kameroppervlakte; {width}x{height}.");

        Id = id;
        Width = width;
        Height = height;

        _fieldGrid = new FieldAdapter[Height, Width];
        _placableGrid = new IPlacable?[Height, Width];

        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
            _fieldGrid[y, x] = new FieldAdapter(this, x, y);
    }

    public void AddItem(IItem item)
    {
        _items.Add(item);

        item.OnItemDepleted += (sender, args) =>
        {
            if (sender is IItem i) _items.Remove(i);
        };
    }

    public void AddEnemy(ILiving enemy)
    {
        _enemies.Add(enemy);
    }

    public void AddDoor(Door door)
    {
        _doors.Add(door);
    }
    
    public void ConnectMechanisms()
    {
        var subjects = Items.OfType<ISubject>().ToList();
        if (subjects.Count == 0) return;

        var observers = Doors.OfType<IObserver>().ToList();
        if (observers.Count == 0) return;

        foreach (var subject in subjects)
        {
            foreach (var observer in observers)
            {
                subject.Attach(observer);
            }
        }
    }

    public void AddSpecialFloorTile(SpecialFloorTile tile)
    {
        _specialFloorTiles.Add(tile);
    }

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        var isWall = x == 0 || x == Width - 1 || y == 0 || y == Height - 1;
        return !isWall || Doors.Any(d => d.X == x && d.Y == y);
    }

    public void OnPlayerEnter(Player player)
    {
        var item = Items.FirstOrDefault(i => i.XPos == player.X && i.YPos == player.Y);

        item?.Interact(player);
    }

    public IPlacable? GetPlacable(int x, int y)
    {
        return IsValidCoordinate(x, y) ? _placableGrid[y, x] : null;
    }

    public void SetPlacable(int x, int y, IPlacable? item)
    {
        if (IsValidCoordinate(x, y)) _placableGrid[y, x] = item;
    }

    public FieldAdapter? GetField(int x, int y)
    {
        return IsValidCoordinate(x, y) ? _fieldGrid[y, x] : null;
    }

    public bool HasSpecialTile(int x, int y, string type)
    {
        return _specialFloorTiles.Any(t => t.X == x && t.Y == y && t.Type == type);
    }

    public void RemoveEnemy(ILiving enemy)
    {
        _enemies.Remove(enemy);

        if (enemy is not (Enemy e and IPlacable placable)) return;
        if (GetPlacable(e.CurrentXLocation, e.CurrentYLocation) == placable)
            SetPlacable(e.CurrentXLocation, e.CurrentYLocation, null);
    }

    private bool IsValidCoordinate(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}