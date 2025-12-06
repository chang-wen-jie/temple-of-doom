using TempleOfDoom.Data.DTOs;
using TempleOfDoom.Data.Strategies;

namespace TempleOfDoom.Data.Loaders;

public class LevelLoader(ILevelLoadStrategy strategy)
{
    private readonly ILevelLoadStrategy _strategy = strategy;
    
    public RootObject LoadLevel(string filePath)
    {
        return _strategy.LoadLevel(filePath);
    }
}