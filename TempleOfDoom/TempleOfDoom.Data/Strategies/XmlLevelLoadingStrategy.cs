using System.Xml.Serialization;
using TempleOfDoom.Data.DTOs;

namespace TempleOfDoom.Data.Strategies;

public class XmlLevelLoadingStrategy : ILevelLoadStrategy
{
    public RootObject LoadLevel(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("Bestand niet gevonden", filePath);

        var serializer = new XmlSerializer(typeof(RootObject));

        using var reader = new StreamReader(filePath);
        return (RootObject)serializer.Deserialize(reader)
               ?? throw new Exception("Parsen van XML gefaald");
    }
}