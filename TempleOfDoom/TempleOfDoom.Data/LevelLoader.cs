using TempleOfDoom.Data.Strategies;

namespace TempleOfDoom.Data;

public class LevelLoader(ILevelLoadStrategy strategy)
{
    private readonly ILevelLoadStrategy _strategy = strategy;
    
    public RootObject LoadLevel(string filePath)
    {
        return _strategy.LoadLevel(filePath);
    }
}