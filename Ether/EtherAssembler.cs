namespace Ether;

public static class EtherAssembler
{
    private const string ReturnMarkName = "return";

    public static IEtherResult Assemble(IReadOnlyCollection<string> mnemonics, int returnOffset)
    {
        mnemonics = mnemonics.Select(x => x.Trim()).ToArray();

        var marks = new JumpMarksManager();
        var data = new DataManager();
        using var instructions = new InstructionsManager(marks, data);

        marks[ReturnMarkName] = returnOffset;
        marks.Initialize(mnemonics);

        foreach (var mnemonic in mnemonics)
        {
            if (DataManager.IsDataMnemonic(mnemonic))
            {
                data.Add(mnemonic);
                continue;
            }

            instructions.Add(mnemonic);
        }

        data.Offset = instructions.Size;

        var instructionsAssembly = instructions.GetAssembly();
        var dataAssembly = data.GetData();
        var assembly = instructionsAssembly.Concat(dataAssembly).ToArray();

        return new EtherResult(assembly, marks.GetMarks());
    }
}