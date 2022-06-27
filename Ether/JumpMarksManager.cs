using System.Text.RegularExpressions;

namespace Ether;

internal sealed class JumpMarksManager
{
    private const string JumpMarkPattern = "#([a-zA-Z][a-zA-Z0-9]{1,18})";

    private readonly Dictionary<string, int> _marks = new();

    public int this[string name] { get => _marks[name]; set => _marks[name] = value; }

    public void Initialize(IReadOnlyCollection<string> mnemonics)
    {
        foreach (var mnemonic in mnemonics)
        {
            TryDefine(mnemonic, 0);
        }
    }

    public bool TryDefine(string mnemonic, int offset)
    {
        var match = Regex.Match(mnemonic, JumpMarkPattern);

        if (match.Success)
        {
            _marks[match.Groups[1].Value] = offset;
        }

        return match.Success;
    }

    public bool ContainsMarks(string name) => _marks.ContainsKey(name);
}