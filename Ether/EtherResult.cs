namespace Ether;

public interface IEtherResult
{
    public byte[] Assembly { get; }
    public IReadOnlyDictionary<string, int> JumpMarks { get; }
}

internal sealed class EtherResult : IEtherResult
{
    public EtherResult(byte[] assembly, IReadOnlyDictionary<string, int> jumpMarks)
    {
        Assembly = assembly;
        JumpMarks = jumpMarks;
    }

    public byte[] Assembly { get; }
    public IReadOnlyDictionary<string, int> JumpMarks { get; }
}