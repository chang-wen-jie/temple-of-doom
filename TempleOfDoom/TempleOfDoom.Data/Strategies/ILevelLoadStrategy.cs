using TempleOfDoom.Data.DTOs;

namespace TempleOfDoom.Data.Strategies;

public interface ILevelLoadStrategy
{
    RootObject LoadLevel(string filePath);
}