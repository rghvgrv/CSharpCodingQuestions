using System.Collections;
using System.Runtime.CompilerServices;

namespace CSharpCodingQuestions.Core;

public static class Formatter
{
    /// <summary>Turns a value into readable text: arrays and lists become [1, 2, 3], strings get quotes.</summary>
    public static string Format(object? value) => value switch
    {
        null => "null",
        string text => $"\"{text}\"",
        char letter => $"'{letter}'",
        bool flag => flag ? "true" : "false",
        double number => number.ToString("0.#####"),
        ITuple tuple => "(" + string.Join(", ", Enumerable.Range(0, tuple.Length).Select(i => Format(tuple[i]))) + ")",
        ListNode or TreeNode => value.ToString()!,
        IEnumerable items => "[" + string.Join(", ", items.Cast<object?>().Select(Format)) + "]",
        _ => value.ToString() ?? "",
    };
}
