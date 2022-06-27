using Ether.Instructions;
using Reloaded.Assembler;

namespace Ether;

internal sealed class InstructionsManager : IDisposable
{
    private const string PseudoAddressOperand = "0x08000000";

    private readonly Assembler _assembler = new();
    private readonly List<Instruction> _instructions = new();
    private readonly JumpMarksManager _marks;
    private readonly DataManager _data;

    public InstructionsManager(JumpMarksManager marks, DataManager data)
    {
        _marks = marks;
        _data = data;
    }

    public void Add(string mnemonic)
    {
        var offset = AcquireOffset();

        if (_marks.TryDefine(mnemonic, offset)) return;
        if (TryAddDataInstruction(mnemonic, offset)) return;

        if (JumpInstruction.TryCreate(mnemonic, offset, _marks, out var instruction))
        {
            _instructions.Add(instruction!);
            return;
        }

        _instructions.Add(AssembleInstruction(mnemonic, assembly => new Instruction(offset, assembly)));
    }

    public byte[] GetAssembly()
    {
        var data = Array.Empty<byte>();
        return _instructions.Aggregate(data, (current, instruction) => current.Concat(instruction.Assemble()).ToArray());
    }

    private bool TryAddDataInstruction(string instruction, int offset)
    {
        foreach (var objectName in _data.ObjectNames)
        {
            if (instruction.Contains(objectName, StringComparison.OrdinalIgnoreCase))
            {
                var dataObject = _data[objectName];

                instruction = instruction.Replace(objectName, PseudoAddressOperand);

                _instructions.Add(AssembleInstruction(instruction, assembly => new DataInstruction(offset, assembly, dataObject)));
                return true;
            }
        }

        return false;
    }

    private Instruction AssembleInstruction(string instruction, Func<byte[], Instruction> create)
    {
        var assembly = _assembler.Assemble(new[] { "use64", instruction });
        return create(assembly);
    }

    private int AcquireOffset()
    {
        if (_instructions.Count is 0) return 0;

        var lastInstruction = _instructions[^1];

        return lastInstruction.Offset + lastInstruction.Size;
    }

    public void Dispose()
    {
        _assembler.Dispose();
    }
}