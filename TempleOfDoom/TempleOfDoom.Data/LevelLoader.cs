using TempleOfDoom.Data.Strategies;

namespace TempleOfDoom.Data;

public class LevelLoader
{
    private ILevelLoadStrategy _strategy;

    public LevelLoader()
    {
        _strategy = new JsonLevelLoadStrategy();
    }
    
    public LevelLoader(ILevelLoadStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void setStrategy(ILevelLoadStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public RootObject LoadLevel(string filePath)
    {
        return _strategy.LoadLevel(filePath);
    }
}