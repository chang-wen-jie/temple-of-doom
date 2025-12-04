using TempleOfDoom.Data.Strategies;

namespace TempleOfDoom.Logic.Models.Factories;

public static class LevelStrategyFactory
{
    public static ILevelLoadStrategy GetStrategy(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Bestandsnaam is leeg");

        if (fileName.EndsWith(".json"))
            return new JsonLevelLoadStrategy();

        if (fileName.EndsWith(".xml"))
            return new XmlLevelLoadingStrategy();

        throw new NotSupportedException($"Bestandstype voor '{fileName}' wordt niet ondersteund.");
    }
}