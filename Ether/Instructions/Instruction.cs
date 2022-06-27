namespace Ether.Instructions;

internal class Instruction
{
    protected readonly byte[] Assembly;

    public Instruction(int offset, byte[] assembly)
    {
        Assembly = assembly;
        Offset = offset;
    }

    public int Offset { get; }
    public int Size => Assembly.Length;

    public virtual byte[] Assemble() => Assembly;
}