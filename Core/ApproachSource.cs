using System.Text.RegularExpressions;

namespace CSharpCodingQuestions.Core;

/// <summary>Cuts a question's source file into one code snippet per [Approach].</summary>
public static partial class ApproachSource
{
    [GeneratedRegex(@"^[ \t]*\[Approach\(", RegexOptions.Multiline)]
    private static partial Regex ApproachStart();

    [GeneratedRegex(@"Name = ""([^""]+)""")]
    private static partial Regex ApproachName();

    [GeneratedRegex(@"\)\][ \t]*$", RegexOptions.Multiline)]
    private static partial Regex AttributeEnd();

    // The runner members that follow the approaches: `public static Example[] Examples` or `public static void Demo()`.
    [GeneratedRegex(@"^[ \t]*public static [^\n]*\b(Examples|Demo)\b", RegexOptions.Multiline)]
    private static partial Regex RunnerStart();

    /// <summary>Returns (approach name, code) pairs in file order. The code excludes the attribute itself.</summary>
    public static List<(string Name, string Code)> Split(string source)
    {
        var starts = ApproachStart().Matches(source);
        Match runner = RunnerStart().Match(source);
        int endOfLast = runner.Success ? runner.Index : source.LastIndexOf('}');

        var snippets = new List<(string Name, string Code)>();
        for (int i = 0; i < starts.Count; i++)
        {
            int start = starts[i].Index;
            int end = i + 1 < starts.Count ? starts[i + 1].Index : endOfLast;
            string segment = source[start..end];

            string name = ApproachName().Match(segment).Groups[1].Value;
            Match attributeEnd = AttributeEnd().Match(segment);
            string code = segment[(attributeEnd.Index + attributeEnd.Length)..];
            snippets.Add((name, Dedent(code)));
        }
        return snippets;
    }

    /// <summary>Removes the shared indentation and the blank lines around the code.</summary>
    static string Dedent(string code)
    {
        string[] lines = code.Trim('\n').Split('\n').Select(line => line.TrimEnd()).ToArray();
        int indent = lines.Where(line => line.Length > 0).Min(line => line.Length - line.TrimStart().Length);
        return string.Join('\n', lines.Select(line => line.Length >= indent ? line[indent..] : line)).Trim('\n');
    }
}
