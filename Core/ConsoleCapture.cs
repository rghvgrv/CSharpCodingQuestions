namespace CSharpCodingQuestions.Core;

/// <summary>
/// Replaces Console.Out so each question run gets its own output buffer.
/// AsyncLocal flows into every Thread, Task and Parallel loop the question starts,
/// so their Console.WriteLine calls land in the same buffer even when many runs happen at once.
/// </summary>
public sealed class ConsoleCapture(TextWriter original) : TextWriter
{
    static readonly AsyncLocal<TextWriter?> currentBuffer = new();

    TextWriter Target => currentBuffer.Value ?? original;

    public override Encoding Encoding => Encoding.UTF8;

    public static void Install() => Console.SetOut(new ConsoleCapture(Console.Out));

    /// <summary>Runs the action and returns what it printed. Throws its exception, or TimeoutException.</summary>
    public static string Run(Action action, TimeSpan timeout)
    {
        var buffer = new StringWriter();
        currentBuffer.Value = TextWriter.Synchronized(buffer);
        try
        {
            Task task = Task.Run(action);
            if (!task.Wait(timeout))
            {
                throw new TimeoutException($"Timed out after {timeout.TotalSeconds} seconds.");
            }
        }
        finally
        {
            currentBuffer.Value = null;
        }
        return buffer.ToString().Replace("\r\n", "\n").TrimEnd();
    }

    public override void Write(char value) => Target.Write(value);

    public override void Write(string? value) => Target.Write(value);

    public override void WriteLine(string? value) => Target.WriteLine(value);

    public override void Flush() => Target.Flush();
}
