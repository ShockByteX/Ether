namespace Ether.Instructions;

internal sealed class JumpInstruction : Instruction
{
    private static readonly byte[] EmptyOperand = { 0, 0, 0, 0 };
    private static readonly Dictionary<string, byte[]> JumpOpCodes = new()
    {
        { "jmp", new byte[] { 0xe9 } },
        { "jb", new byte[] { 0x0f, 0x82 } },
        { "jae", new byte[] { 0x0f, 0x83 } },
        { "je", new byte[] { 0x0f, 0x84 } },
        { "jne", new byte[] { 0x0f, 0x85 } },
        { "jbe", new byte[] { 0x0f, 0x86 } },
        { "ja", new byte[] { 0x0f, 0x87 } },
        { "jl", new byte[] { 0x0f, 0x8c } },
        { "jge", new byte[] { 0x0f, 0x8d } },
        { "jle", new byte[] { 0x0f, 0x8e } }
    };

    private readonly Func<int> _getJumpOffset;

    public JumpInstruction(int offset, byte[] assembly, Func<int> getJumpOffset) : base(offset, assembly)
    {
        _getJumpOffset = getJumpOffset;
    }

    public override byte[] Assemble()
    {
        var delta = _getJumpOffset() - (Offset + Size);
        var operandData = BitConverter.GetBytes(delta);

        return Assembly[..^4].Concat(operandData).ToArray();
    }

    public static bool TryCreate(string mnemonic, int offset, JumpMarksManager marks, out JumpInstruction? instruction)
    {
        instruction = null;

        var definition = mnemonic.Split(' ');

        if (definition.Length is not 2) return false;

        var operation = definition[0];

        if (!JumpOpCodes.TryGetValue(operation, out var opCode)) return false;

        var operand = definition[1];

        if (!marks.ContainsMarks(operand)) return false;

        var assembly = opCode.Concat(EmptyOperand).ToArray();

        instruction = new JumpInstruction(offset, assembly, () => marks[operand]);

        return true;
    }
}