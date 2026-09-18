namespace CSharpCodingQuestions.Core;

/// <summary>Printing helper for demo questions (the ones without fixed inputs, like the parallelism questions).</summary>
public static class Output
{
    public const string MismatchMarker = "MISMATCH:";

    /// <summary>Prints "label: value". When <paramref name="expected"/> is given and differs, the check run fails.</summary>
    public static void Print(string label, object? value, object? expected = null)
    {
        Console.WriteLine($"{label}: {(value is string text ? text : Formatter.Format(value))}");
        if (expected != null && Formatter.Format(value) != Formatter.Format(expected))
        {
            Console.WriteLine($"{MismatchMarker} expected {Formatter.Format(expected)}");
        }
    }
}
