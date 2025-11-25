using System.Xml.Serialization;

namespace TempleOfDoom.Data.Strategies;

public class XmlLevelLoadingStrategy : ILevelLoadStrategy
{
    public RootObject LoadLevel(string filePath)
    {
        if (!File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

        var serializer = new XmlSerializer(typeof(RootObject));

        using var reader = new StreamReader(filePath);
        return (RootObject)serializer.Deserialize(reader) 
               ?? throw new Exception("Failed to parse XML");
    }
}