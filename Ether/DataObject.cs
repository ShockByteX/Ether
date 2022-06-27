using System.Runtime.InteropServices;
using Ether.Marshaling;

namespace Ether;

internal sealed class DataObject
{
    private readonly DataManager _data;
    private readonly object _value;
    
    public DataObject(DataManager data, int offset, object value)
    {
        _data = data;
        _value = value;
        Offset = offset;
        Size = Marshal.SizeOf(_value.GetType());
    }

    public int Offset { get; }
    public int Size { get; }
    public int AbsoluteOffset => _data.Offset + Offset;

    public byte[] GetData() => MarshalType.Convert(_value);
}