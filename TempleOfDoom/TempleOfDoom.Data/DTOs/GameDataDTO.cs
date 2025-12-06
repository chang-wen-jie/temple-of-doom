namespace TempleOfDoom.Data.DTOs;

public class RootObject
{
    public RoomDto[] Rooms { get; set; }
    public ConnectionDto[] Connections { get; set; }
    public PlayerDto Player { get; set; }
}

public class RoomDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public ItemDto[]? Items { get; set; }
    public SpecialFloorTileDto[]? SpecialFloorTiles { get; set; }
    public EnemyDto[]? Enemies { get; set; }
}

public class ItemDto
{
    public string Type { get; set; }
    public int Damage { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string Color { get; set; }
}

public class SpecialFloorTileDto
{
    public string Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class EnemyDto
{
    public string Type { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int MinX { get; set; }
    public int MinY { get; set; }
    public int MaxX { get; set; }
    public int MaxY { get; set; }
}

public class ConnectionDto
{
    public int North { get; set; }
    public int South { get; set; }
    public DoorDto[]? Doors { get; set; }
    public int West { get; set; }
    public int East { get; set; }
    public int Upper { get; set; }
    public int Lower { get; set; }
    public LadderDto Ladder { get; set; }
}

public class DoorDto
{
    public string Type { get; set; }
    public string? Color { get; set; }
    public int NoOfStones { get; set; }
}

public class LadderDto
{
    public int UpperX { get; set; }
    public int UpperY { get; set; }
    public int LowerX { get; set; }
    public int LowerY { get; set; }
}

public class PlayerDto
{
    public int StartRoomId { get; set; }
    public int StartX { get; set; }
    public int StartY { get; set; }
    public int Lives { get; set; }
}