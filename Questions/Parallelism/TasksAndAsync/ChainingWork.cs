namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 5, Title = "Do This, Then That (Continuations)", Level = Medium, Problem = """
    Three steps must happen in order in the background: download some text, parse it into a number, then double it.
    Also handle the case where parsing fails.
    """)]
public static class ChainingWork
{
    [Approach(Name = "ContinueWith", Idea = """
        Before `async/await` existed, you chained tasks with `ContinueWith`: "when this task is done, run that".
        It works, but each step has to dig the value out with `.Result`, errors arrive wrapped in an `AggregateException`,
        and longer chains get hard to read.
        """)]
    public static Task<string> ProcessWithContinueWith(string input)
    {
        return Task.Run(() => input)
            .ContinueWith(download => int.Parse(download.Result))
            .ContinueWith(parse =>
            {
                if (parse.IsFaulted)
                {
                    return $"error: {parse.Exception!.InnerException!.GetType().Name}";
                }
                return (parse.Result * 2).ToString();
            });
    }

    [Approach(Name = "async / await", Idea = """
        `await` builds the same chain for you, but the code reads top to bottom like normal code,
        and a regular `try/catch` handles errors. Prefer this in new code, and recognize `ContinueWith` in older code.
        """)]
    public static async Task<string> ProcessWithAwait(string input)
    {
        try
        {
            string text = await Task.Run(() => input);
            int number = await Task.Run(() => int.Parse(text));
            return (number * 2).ToString();
        }
        catch (FormatException error)
        {
            return $"error: {error.GetType().Name}";
        }
    }

    public static void Demo()
    {
        Print("ContinueWith(\"21\")", ProcessWithContinueWith("21").Result, expected: "42");
        Print("ContinueWith(\"oops\")", ProcessWithContinueWith("oops").Result, expected: "error: FormatException");
        Print("await(\"21\")", ProcessWithAwait("21").Result, expected: "42");
        Print("await(\"oops\")", ProcessWithAwait("oops").Result, expected: "error: FormatException");
    }
}
