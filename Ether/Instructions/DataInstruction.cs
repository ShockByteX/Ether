namespace Ether.Instructions;

internal sealed class DataInstruction : Instruction
{
    private readonly DataObject _dataObject;

    public DataInstruction(int offset, byte[] assembly, DataObject dataObject) : base(offset, assembly)
    {
        _dataObject = dataObject;
    }

    public override byte[] Assemble()
    {
        var offset = _dataObject.AbsoluteOffset - (Offset + Size);
        return Assembly[..^4].Concat(BitConverter.GetBytes(offset)).ToArray();
    }
}