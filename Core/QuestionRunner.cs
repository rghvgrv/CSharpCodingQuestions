using System.Collections;
using System.Reflection;

namespace CSharpCodingQuestions.Core;

/// <summary>
/// Runs a question. With Examples: every approach runs on every input and must return the expected answer;
/// the page shows the best approach's output. Without: Demo() runs and the page shows what it printed.
/// </summary>
public static class QuestionRunner
{
    static readonly TimeSpan timeout = TimeSpan.FromSeconds(20);
    static readonly ConcurrentDictionary<string, Lazy<QuestionResult>> cache = new();

    /// <summary>Runs the question the first time it's asked for, then reuses that result.</summary>
    public static QuestionResult GetResult(QuestionInfo question) =>
        cache.GetOrAdd(question.Id, _ => new Lazy<QuestionResult>(() => Run(question))).Value;

    public static QuestionResult Run(QuestionInfo question)
    {
        var examples = new List<ExampleResult>();
        var errors = new List<string>();
        string text = "";
        try
        {
            text = ConsoleCapture.Run(() =>
            {
                if (question.HasExamples)
                {
                    RunExamples(question, examples, errors);
                }
                else
                {
                    question.Type.GetMethod("Demo")!.Invoke(null, null);
                }
            }, timeout);
        }
        catch (Exception exception)
        {
            errors.Add(Unwrap(exception).ToString());
        }

        if (text.Contains(Output.MismatchMarker))
        {
            errors.Add("Demo printed a value different from the expected one.");
        }
        return new QuestionResult(examples, text, errors);
    }

    static void RunExamples(QuestionInfo question, List<ExampleResult> results, List<string> errors)
    {
        MethodInfo best = question.Approaches[^1].Method!;
        string[] parameterNames = best.GetParameters().Select(parameter => parameter.Name!).ToArray();
        int exampleCount = LoadExamples(question).Length;

        for (int i = 0; i < exampleCount; i++)
        {
            Example example = LoadExamples(question)[i];
            string input = string.Join(", ", parameterNames.Zip(example.Input, (name, value) => $"{name} = {Formatter.Format(value)}"));
            string output = "";

            foreach (ApproachInfo approach in question.Approaches)
            {
                // Fresh input objects for every approach, because some approaches change their input.
                Example fresh = LoadExamples(question)[i];
                object? actual = Unwrapped(() => approach.Method!.Invoke(null, fresh.Input));
                if (!Matches(actual, example.Expected, example.AnyOrder))
                {
                    errors.Add($"{approach.Method!.Name}({input}) returned {Formatter.Format(actual)}, expected {Formatter.Format(example.Expected)}");
                }
                output = Formatter.Format(actual);
            }
            results.Add(new ExampleResult(input, output));
        }
    }

    static Example[] LoadExamples(QuestionInfo question) =>
        (Example[])question.Type.GetProperty("Examples")!.GetValue(null)!;

    static bool Matches(object? actual, object? expected, bool anyOrder)
    {
        if (Formatter.Format(actual) == Formatter.Format(expected))
        {
            return true;
        }
        if (!anyOrder || actual is not IEnumerable actualItems || expected is not IEnumerable expectedItems)
        {
            return false;
        }
        return Sorted(actualItems).SequenceEqual(Sorted(expectedItems));

        static IEnumerable<string> Sorted(IEnumerable items) => items.Cast<object?>().Select(Formatter.Format).Order();
    }

    static object? Unwrapped(Func<object?> call)
    {
        try
        {
            return call();
        }
        catch (TargetInvocationException exception)
        {
            throw exception.InnerException!;
        }
    }

    static Exception Unwrap(Exception exception) => exception switch
    {
        AggregateException { InnerException: { } inner } => Unwrap(inner),
        TargetInvocationException { InnerException: { } inner } => Unwrap(inner),
        _ => exception,
    };
}
