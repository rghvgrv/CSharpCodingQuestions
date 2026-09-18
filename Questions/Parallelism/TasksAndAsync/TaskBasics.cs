namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 1, Title = "Get a Result From Background Work (Task<T>)", Level = Easy, Problem = """
    Add up the numbers 1 to 1,000,000 on a background thread and get the result back.
    """)]
public static class TaskBasics
{
    [Approach(Name = "Thread + Shared Variable", Idea = """
        A `Thread` can't return a value. So the thread writes into a variable outside it, and you must remember to `Join()`
        before reading that variable, or you may read it too early. Errors thrown inside the thread are also hard to catch.
        """)]
    public static long SumWithThread()
    {
        long result = 0;
        var thread = new Thread(() =>
        {
            long sum = 0;
            for (int i = 1; i <= 1_000_000; i++)
            {
                sum += i;
            }
            result = sum;
        });
        thread.Start();
        thread.Join();
        return result;
    }

    [Approach(Name = "Task<T>", Idea = """
        `Task.Run` runs the work on a thread-pool thread and gives back a `Task<long>`: a **promise** of a future result.

        - `await task` (or `.Result` in non-async code) gives you the value when it's ready.
        - If the work throws, the exception comes out of `await`, so a normal `try/catch` works.
        - Tasks can be combined, cancelled and chained, as the next questions show.
        """)]
    public static long SumWithTask()
    {
        Task<long> task = Task.Run(() =>
        {
            long sum = 0;
            for (int i = 1; i <= 1_000_000; i++)
            {
                sum += i;
            }
            return sum;
        });
        return task.Result;
    }

    public static void Demo()
    {
        Print("Thread + shared variable", SumWithThread(), expected: 500_000_500_000L);
        Print("Task<long>", SumWithTask(), expected: 500_000_500_000L);

        Task<int> failing = Task.Run(() => int.Parse("not a number"));
        try
        {
            failing.Wait();
        }
        catch (AggregateException error)
        {
            Print("A task's exception reaches the caller", error.InnerException!.GetType().Name, expected: "FormatException");
        }
    }
}
