using Ether.Validation;

namespace Ether;

internal sealed class DataManager
{
    private static readonly Dictionary<string, Func<string, object>> TypesMapping = new()
    {
        { "int", text => int.Parse(text) },
        { "long", text => long.Parse(text) },
        { "float", text => float.Parse(text) },
        { "double", text => double.Parse(text) },
    };

    private readonly Dictionary<string, DataObject> _objects = new();

    public int Offset { get; set; }
    public IReadOnlyCollection<string> ObjectNames => _objects.Keys;
    public DataObject this[string name] => _objects[name];

    public void Add(string line)
    {
        var definition = line.Split(' ');
        var typeName = definition[0];

        Assert.That(TypesMapping.TryGetValue(typeName, out var parse), $"Invalid type: {typeName}");
        Assert.That(definition.Length == 3, $"Definition parts is not equals 3: '{line}'");

        var name = definition[1];

        Assert.That(!_objects.ContainsKey(name), $"You have been already created variable with name: {name}");

        var value = parse!(definition[2]);
        var lastObject = _objects.Values.LastOrDefault();
        var offset = lastObject is null ? 0 : lastObject.Offset + lastObject.Size;

        _objects.Add(name, new DataObject(this, offset, value));
    }

    public byte[] GetData()
    {
        var data = Array.Empty<byte>();
        return _objects.Values.Aggregate(data, (current, dataObject) => current.Concat(dataObject.GetData()).ToArray());
    }

    public static bool IsDataMnemonic(string mnemonic)
    {
        var definition = mnemonic.Split(' ');
        var typeName = definition[0];

        return TypesMapping.ContainsKey(typeName);
    }
}