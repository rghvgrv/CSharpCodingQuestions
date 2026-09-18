using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CodingQuestions;

public enum Level { Easy, Medium, Hard }

/// <summary>Marks a class as a question. Order = section_topic_number, e.g. 1_02_03.</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class QAttribute(int order, string title, Level level, string question) : Attribute
{
    public int Order { get; } = order;
    public string Title { get; } = title;
    public Level Level { get; } = level;
    public string Question { get; } = question;
}

public record Question(int No, string Id, string Section, string Topic, string Title, Level Level, string Text, Type Type);

public static class Questions
{
    // Section and topic come from the namespace: CodingQuestions.<Section>.<Topic>
    public static readonly Question[] All = typeof(Questions).Assembly.GetTypes()
        .Select(t => (t, a: t.GetCustomAttribute<QAttribute>()))
        .Where(x => x.a != null)
        .OrderBy(x => x.a!.Order)
        .Select((x, i) =>
        {
            var ns = x.t.Namespace!.Split('.');
            return new Question(i + 1, x.t.Name, Pretty(ns[1]), Pretty(ns[2]), x.a!.Title, x.a.Level, x.a.Question, x.t);
        })
        .ToArray();

    public static readonly Dictionary<string, Question> ById = All.ToDictionary(q => q.Id, StringComparer.OrdinalIgnoreCase);

    // "Dsa" → "DSA", "StacksAndQueues" → "Stacks & Queues", "LinkedList" → "Linked List"
    static string Pretty(string s) => s == "Dsa" ? "DSA" : Regex.Replace(s, "(?<!^)(And)?([A-Z])", m => (m.Groups[1].Success ? " &" : "") + " " + m.Groups[2].Value);

    public static string ReadSource(string name)
    {
        using var s = typeof(Questions).Assembly.GetManifestResourceStream(name);
        return s == null ? "// source not found" : new StreamReader(s).ReadToEnd();
    }

    /// <summary>Runs a question's static Run() and captures everything it writes to Console, from any thread.</summary>
    public static (string Output, double Ms) Run(Question q)
    {
        var sw = new StringWriter();
        RoutedConsole.Current.Value = TextWriter.Synchronized(sw);
        var run = q.Type.GetMethod("Run")!.CreateDelegate<Action>();
        var clock = Stopwatch.StartNew();
        try
        {
            if (!Task.Run(run).Wait(TimeSpan.FromSeconds(20))) Console.WriteLine("⏱ timed out after 20s");
        }
        catch (AggregateException e) { Console.WriteLine($"💥 {e.InnerException}"); }
        finally { RoutedConsole.Current.Value = null; }
        return (sw.ToString(), clock.Elapsed.TotalMilliseconds);
    }
}

/// <summary>Console.Out replacement: each run gets its own buffer (AsyncLocal flows into Tasks, Threads and Parallel loops).</summary>
public sealed class RoutedConsole(TextWriter fallback) : TextWriter
{
    public static readonly AsyncLocal<TextWriter?> Current = new();
    TextWriter Target => Current.Value ?? fallback;
    public override Encoding Encoding => Encoding.UTF8;
    public override void Write(char value) => Target.Write(value);
    public override void Write(string? value) => Target.Write(value);
    public override void WriteLine(string? value) => Target.WriteLine(value);
    public override void Flush() => Target.Flush();
}

/// <summary>Shared helpers used by the questions.</summary>
public static class Lib
{
    /// <summary>Formats values for printing: arrays/lists as [1, 2, 3], nested too.</summary>
    public static string Fmt(object? x) => x switch
    {
        null => "null",
        string s => s,
        bool b => b ? "true" : "false",
        double d => d.ToString("0.#####"),
        IEnumerable e => "[" + string.Join(", ", e.Cast<object?>().Select(Fmt)) + "]",
        _ => x.ToString() ?? ""
    };

    /// <summary>Prints the call and its result, with ✓ / ✗ against the expected answer.</summary>
    public static void Check<T>(string call, T actual, T expected)
    {
        string a = Fmt(actual), e = Fmt(expected);
        Console.WriteLine(a == e ? $"✓ {call} → {a}" : $"✗ {call} → {a}   (expected {e})");
    }

    /// <summary>Prints the call and its result (for answers with no single expected value).</summary>
    public static void Show(string call, object? result) => Console.WriteLine($"• {call} → {Fmt(result)}");
}

public class ListNode(int val, ListNode? next = null)
{
    public int Val = val;
    public ListNode? Next = next;

    public static ListNode? From(params int[] values)
    {
        ListNode? head = null;
        for (int i = values.Length - 1; i >= 0; i--) head = new ListNode(values[i], head);
        return head;
    }

    public override string ToString()
    {
        var parts = new List<int>();
        for (ListNode? n = this; n != null && parts.Count < 100; n = n.Next) parts.Add(n.Val);
        return string.Join(" → ", parts);
    }
}

public class TreeNode(int val, TreeNode? left = null, TreeNode? right = null)
{
    public int Val = val;
    public TreeNode? Left = left, Right = right;

    /// <summary>Builds a tree from LeetCode-style level order: [1, 2, 3, null, 4].</summary>
    public static TreeNode? From(params int?[] values)
    {
        if (values.Length == 0 || values[0] == null) return null;
        var root = new TreeNode(values[0]!.Value);
        var q = new Queue<TreeNode>([root]);
        for (int i = 1; i < values.Length; i += 2)
        {
            var node = q.Dequeue();
            if (values[i] is int l) q.Enqueue(node.Left = new TreeNode(l));
            if (i + 1 < values.Length && values[i + 1] is int r) q.Enqueue(node.Right = new TreeNode(r));
        }
        return root;
    }

    /// <summary>Level-order form, trailing nulls trimmed: [1, 2, 3, null, 4].</summary>
    public override string ToString()
    {
        var res = new List<string>();
        var q = new Queue<TreeNode?>([this]);
        while (q.Count > 0)
        {
            var n = q.Dequeue();
            res.Add(n == null ? "null" : n.Val.ToString());
            if (n != null) { q.Enqueue(n.Left); q.Enqueue(n.Right); }
        }
        while (res[^1] == "null") res.RemoveAt(res.Count - 1);
        return "[" + string.Join(", ", res) + "]";
    }
}
